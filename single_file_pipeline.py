############################################################
#
# Secure design pattern and vulnerabilities detection system
#
# (c) Miroslaw Staron, 2021-2023
# www.staron.nu
#
#############################################################
#
# This file is for analyzing files one by one
# It creates the embeddings database and then calculates the distances
# one file at a time, at the same time providing the output one file by one
#
# It is better for larger systems, where we can get the results while they are being calculated
#
#############################################################

#
# Algorithm to implement:
# 1. Calculate embeddings for the reference code
# 2. For each analyzed file
# 2a. Calculate embeddings for the analyzed code
# 2b. Calculate the distances between the reference code and the analyzed code
# 2c. Print the results

from transformers import RobertaTokenizer
from transformers import RobertaConfig
from transformers import pipeline
import numpy as np
import pandas as pd
import os
from scipy import spatial
import cylberta_embeddings
import codebert_embeddings
import singberta_embeddings

# predictions for small files
def predict_small(code, model, vulnerability, steps):
    steps = []

    try:
        # do something with code and model
        if model == 'codebert' or model == 'singberta':
            snippet_embeddings = codebert_embeddings.extract_embeddings_codebert_from_string(code) if model == 'codebert' else singberta_embeddings.extract_embeddings_singberta_from_string(code)

            steps.append(f'extracted embeddings from {model}')

            # now read the embeddings of the reference files from database/embeddings_codebert_input_validation.csv
            dfRefEmbeddings = pd.read_csv(f'database/embeddings_{model}.csv', sep='$', index_col=None) 

            steps.append('read reference embeddings: ' + str(dfRefEmbeddings.shape[0]) + ' rows ' + str(dfRefEmbeddings.shape[1]) + ' columns')
            
            # select only the rows that contain the vulnerability
            dfRefEmbeddings = dfRefEmbeddings[dfRefEmbeddings['vulnerability'] == vulnerability]

            steps.append('selected only the rows that contain the vulnerability')
            
            # if the number of rows in dfRefEmbeddings is 0, then the model is not supported
            if dfRefEmbeddings.shape[0] == 0:
                vulnerability_result = 'Error: vulnerability not supported'
                steps.append('vulnebility not supported: ' + vulnerability)
                return flask.jsonify({'result': vulnerability_result, 
                                    'trace': steps})    

            # change the reference embeddings to a dictionary
            # print(dfRefEmbeddings.columns)
            dfRefEmbeddings.set_index('filename', inplace=True)

            dictReferenceEmbeddings = dfRefEmbeddings.drop(['vulnerability'], axis=1).T.to_dict('list')

            steps.append('created reference dictionary')

            # calculate the distances between the reference and analyzed code
            dfDistances = calculate_distances_from_dict(dictReferenceEmbeddings, {'ME': snippet_embeddings})

            steps.append('calculated distances')

            # check vulnerability
            vulnerability_result, lstReferences = check_vulnerability(dfDistances)

            steps.append('checked vulnerability')

            # return a JSON string
            return {'result': vulnerability_result, 
                    'references': lstReferences,
                    'lines': code}
        else:
            return {'result': 'Error: model not supported',
                    'trace': steps}
    except Exception as e:
        return {'result': 'Error: problem with processing the code or model not available', 
                'Error': str(e),
                'references': steps}

# predictions for large files
def predict_large(code, model, vulnerability, steps, chunk_size):
    steps = steps
    i = 0

    steps.append('Large file endpoint')
    
    dictResult = {}
    
    # divide the list code into smaller parts of 50 lines
    
    # divide the list code into smaller parts of 50 lines
    chunks = [code[i:i + chunk_size] for i in range(0, len(code), chunk_size)]

    for oneChunk in chunks:
        dictResult[i] = predict_small(oneChunk, model, vulnerability, steps)
        i = i+1
        
    
    return dictResult

# calculate the reference embeddings that we can use for calculating the distances
def calculate_reference_embeddings(strReferenceFolder,
                                   strModel):
    '''
    This function is used to calculate the reference embeddings.
    '''

    print(f'Folder: {strReferenceFolder}')

    if strModel == 'cylbert':
        dictReferenceEmbeddings = cylberta_embeddings.extract_embeddings_cylbert_dict(strReferenceFolder)
    elif strModel == 'codebert':
        dictReferenceEmbeddings = codebert_embeddings.extract_embeddings_codebert_dict(strReferenceFolder)
    elif strModel == 'singberta':
        dictReferenceEmbeddings = singberta_embeddings.extract_embeddings_singberta_dict(strReferenceFolder)
    else:
        dictReferenceEmbeddings = {}

    return dictReferenceEmbeddings

# calculate the distances between the reference and analyzed code
def calculate_distances(dictReferenceEmbeddings, 
                        dictAnalyzedEmbeddings):
    '''
    This function is used to calculate the distances between the reference and analyzed code.
    '''
    lstDistances = []

    for strFile in dictAnalyzedEmbeddings.keys():
        
        lstAnalyzedEmbeddings = dictAnalyzedEmbeddings[strFile]
    
        for strReferenceFile in dictReferenceEmbeddings.keys():
            # if the filename starts with SCE_ or VCE_
            if 'SCE_' in strReferenceFile or 'VCE_' in strReferenceFile:
                lstReferenceEmbeddings = list(dictReferenceEmbeddings[strReferenceFile].values())

                print(lstReferenceEmbeddings)

                # calculate the distances
                fltDistance = spatial.distance.cosine(lstAnalyzedEmbeddings, lstReferenceEmbeddings)

                lstDistances.append([strFile, strReferenceFile, fltDistance])
    
    dfDistances = pd.DataFrame(lstDistances, columns=['Module', 'Reference', 'Distance'])
    
    return dfDistances
    
# print the analysis to the screen and then to the csv file
def print_results(dfDistances, 
                  csvFile):
    '''
    This function is used to print the results.
    '''
    
    # for each module, visualize the distances 
    # between that module and the reference code samples

    dfDistances.reset_index(inplace=True)

    # get unique list of modules
    modules = dfDistances.Module.unique()

    # for each module - find the three closest reference programs
    # and check if they are SCEs or VCEs
    for module in modules:
        # get the distances for the current module
        dfModule = dfDistances[dfDistances['Module'] == module]
        
        # sort the distances
        dfModule = dfModule.sort_values(by=['Distance'])
        
        # get the top 3
        dfModule = dfModule.head(3)
        
        # get the names of the top 3
        dfModuleNames = dfModule['Module']
        
        # get the distances of the top 3
        dfModuleDistances = dfModule['Distance']
        
        # get the labels of the top 3
        dfReferenceLabels = dfModule['Reference']

        # print(dfModuleLabels)

        # check if they are SCEs or VCEs
        iSCEs = 0
        iVCEs = 0
        for label in dfReferenceLabels:
            if 'SCE_' in label:
                iSCEs += 1
            else:
                iVCEs += 1  
    
        # write the verdict
        strVerdict = 'secure' if iVCEs < iSCEs else 'vulnerable'  

        mName = module.split('/')[-1]

        # print the verdict
        # changed to printing only the violations and then the examples
        
        print('*****')
        print(f'Module {mName} is flagged as {strVerdict}, with the following reference examples:')

        # here we print only the vulnerable modules and not all three
        # in order to not confuse the user
        strRefLabels = ''
        for label in dfReferenceLabels:
            lName = label.split("/")[-1] if "/" in label else label.split("\\")[-1]
            print(f'>>>> {lName}')
            strRefLabels = strRefLabels + ';' + lName

        # save the results to the csvFile file
        with open(csvFile, 'a') as f:
            f.write(f'{mName},{strVerdict}{strRefLabels}\n')

    return 1

# print the analysis to the screen and then to the csv file
def check_vulnerability(dfDistances):
    '''
    This function is used to check if the analyzed code is vulnerable or not.
    '''
    
    # for each module, visualize the distances 
    # between that module and the reference code samples

    dfDistances.reset_index(inplace=True)

    # get unique list of modules
    modules = dfDistances.Module.unique()

    # for each module - find the three closest reference programs
    # and check if they are SCEs or VCEs
    for module in modules:
        # get the distances for the current module
        dfModule = dfDistances[dfDistances['Module'] == module]
        
        # sort the distances
        dfModule = dfModule.sort_values(by=['Distance'])
        
        # get the top 3
        dfModule = dfModule.head(3)
        
        # get the names of the top 3
        dfModuleNames = dfModule['Module']
        
        # get the distances of the top 3
        dfModuleDistances = dfModule['Distance']
        
        # get the labels of the top 3
        dfReferenceLabels = dfModule['Reference']

        # print(dfModuleLabels)

        # check if they are SCEs or VCEs
        iSCEs = 0
        iVCEs = 0
        for label in dfReferenceLabels:
            if 'SCE_' in label:
                iSCEs += 1
            else:
                iVCEs += 1  
    
        # write the verdict
        strVerdict = 'secure' if iVCEs < iSCEs else 'vulnerable'  

        mName = module.split('/')[-1]

        # print the verdict
        # changed to printing only the violations and then the examples
        
        #print('*****')
        #print(f'Module {mName} is flagged as {strVerdict}, with the following reference examples:')

        lstReferences = []

        # here we print only the vulnerable modules and not all three
        # in order to not confuse the user
        strRefLabels = ''
        for label in dfReferenceLabels:
            lName = label.split("/")[-1] if "/" in label else label.split("\\")[-1]
        #    print(f'>>>> {lName}')
            strRefLabels = strRefLabels + ';' + lName
            lstReferences.append(lName) 

    return strVerdict, lstReferences

# the entire analysis pipeline
def pipeline(strReferenceFolder, 
             strCodeFolder, 
             strResultFile,
             strResultFolder,  
             strModel):
    '''
    This function is used to calculate the embeddings for the reference code and then
    for each analyzed file, calculate the embeddings and the distances.
    '''
    # calculate the reference embeddings
    dictReferenceEmbeddings = calculate_reference_embeddings(strReferenceFolder, strModel)

    # save reference embeddings to a file
    dfAllEmbeddings = pd.DataFrame.from_dict(dictReferenceEmbeddings, orient='index')

    dfAllEmbeddings.to_csv(os.path.join(strResultFolder, f'embeddings_{strModel}.csv'), sep='$')

    dfAllDistances = pd.DataFrame(data=[], columns=['Module', 'Reference', 'Distance'])

    # for each file in the code folder, extract the embeddings and calculate the distances
    for root, dirs, files in os.walk(strCodeFolder):
        #for strFile in os.listdir(strCodeFolder):
        for strFile in files:
            
            print(f'>>> Processing File: {strFile}')

            # calculate the embeddings for the analyzed code
            if strModel == 'codebert':
                lstEmbeddings = codebert_embeddings.extract_embeddings_codebert_one_file(os.path.join(root, strFile))
            elif strModel == 'cylbert':
                lstEmbeddings = cylberta_embeddings.extract_embeddings_cylbert_one_file(os.path.join(root, strFile))
            elif strModel == 'singberta':
                lstEmbeddings = singberta_embeddings.extract_embeddings_singberta_one_file(os.path.join(root, strFile))
            else:
                lstEmbeddings = []

            dictAnalyzedEmbeddings = {}

            dictAnalyzedEmbeddings[strFile] = lstEmbeddings

            dfAnalyzedEmbeddings = pd.DataFrame.from_dict(dictAnalyzedEmbeddings, orient='index')

            # add the dfAnalyzedEmbeddings to the dfAllEmbeddings dataframe
            dfAllEmbeddings = pd.concat([dfAllEmbeddings, dfAnalyzedEmbeddings])

            # save all embeddings to csv
            dfAllEmbeddings.to_csv(os.path.join(strResultFolder, f'results/embeddings_{strModel}.csv'), sep='$')

            # calculate the distances between the reference and analyzed code
            dfDistances = calculate_distances(dictReferenceEmbeddings, 
                                            dictAnalyzedEmbeddings)
            
            dfAllDistances = pd.concat([dfAllDistances, dfDistances])

            # save distances to file
            dfAllDistances.to_csv(os.path.join(strResultFolder, f'results/distances_{strModel}.csv'), sep='$')

            # print the results
            print_results(dfDistances,strResultFile)

def calculate_distances_from_dict(dictReferenceEmbeddings, dictCodeEmbeddings):
    ''' Takes two dictionaries of embeddings as input and calculates the distances
        between the analyzed code and the reference code. The output is a dataframe
        with the distances between the analyzed code and the reference code. '''

    # List of lists with the results
    lstRes = []

    for refKeys in dictReferenceEmbeddings.keys():
        for modKeys in dictCodeEmbeddings.keys():
            distance = spatial.distance.cosine(dictReferenceEmbeddings[refKeys], dictCodeEmbeddings[modKeys])
            oneDistance = [refKeys, modKeys, distance]
            lstRes.append(oneDistance)

    dfDistances = pd.DataFrame(lstRes, columns=['Reference', 'Module', 'Distance'])

    return dfDistances