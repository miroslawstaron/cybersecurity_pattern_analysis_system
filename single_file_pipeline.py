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
import codex_embeddings

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

# calculate the reference embeddings that we can use for calculating the distances
def calculate_reference_embeddings_codex(strReferenceFolder, strCodeXKeyFile):
    '''
    This function is used to calculate the reference embeddings.
    '''

    dictReferenceEmbeddings = codex_embeddings.extract_embeddings_codex_dict(strReferenceFolder, strCodeXKeyFile)

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
            lstReferenceEmbeddings = dictReferenceEmbeddings[strReferenceFile]

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
        for label in dfReferenceLabels:
            print(f'>>>> {label.split("/")[-1]}')

        # save the results to the csvFile file
        with open(csvFile, 'a') as f:
            f.write(f'{mName},{strVerdict}\n')

    return 1

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

    dfAllDistances = pd.DataFrame(data=[], columns=['Module', 'Reference', 'Distance'])

    # for each file in the code folder, extract the embeddings and calculate the distances
    for strFile in os.listdir(strCodeFolder):
        
        if os.path.isfile(strFile):
            print(f'>>> Processing File: {strFile}')

            # calculate the embeddings for the analyzed code
            if strModel == 'codebert':
                lstEmbeddings = codebert_embeddings.extract_embeddings_codebert_one_file(os.path.join(strCodeFolder, strFile))
            elif strModel == 'cylbert':
                lstEmbeddings = cylberta_embeddings.extract_embeddings_cylbert_one_file(os.path.join(strCodeFolder, strFile))
            elif strModel == 'singberta':
                lstEmbeddings = singberta_embeddings.extract_embeddings_singberta_one_file(os.path.join(strCodeFolder, strFile))
            else:
                lstEmbeddings = []

            dictAnalyzedEmbeddings = {}

            dictAnalyzedEmbeddings[strFile] = lstEmbeddings

            dfAnalyzedEmbeddings = pd.DataFrame.from_dict(dictAnalyzedEmbeddings, orient='index')

            # add the dfAnalyzedEmbeddings to the dfAllEmbeddings dataframe
            dfAllEmbeddings = pd.concat([dfAllEmbeddings, dfAnalyzedEmbeddings])

            # save all embeddings to csv
            dfAllEmbeddings.to_csv(os.path.join(strResultFolder, 'results/embeddings.csv'), sep='$')

            # calculate the distances between the reference and analyzed code
            dfDistances = calculate_distances(dictReferenceEmbeddings, 
                                            dictAnalyzedEmbeddings)
            
            dfAllDistances = pd.concat([dfAllDistances, dfDistances])

            # save distances to file
            dfAllDistances.to_csv(os.path.join(strResultFolder, 'results/distances.csv'), sep='$')

            # print the results
            print_results(dfDistances,strResultFile)


def pipeline_codex(strReferenceFolder, 
                    strCodeFolder, 
                    strResultFile,
                    strResultFolder,
                    strCodeXKeyFile):
    # calculate the reference embeddings
    dictReferenceEmbeddings = calculate_reference_embeddings_codex(strReferenceFolder, strCodeXKeyFile)

    # save reference embeddings to a file
    dfAllEmbeddings = pd.DataFrame.from_dict(dictReferenceEmbeddings, orient='index')

    # for each file in the code folder, extract the embeddings and calculate the distances
    for strFile in os.listdir(strCodeFolder):
        
        # calculate the embeddings for the analyzed code
        lstEmbeddings = codex_embeddings.extract_embeddings_codex_one_file(os.path.join(strCodeFolder, strFile), strCodeXKeyFile)

        dictAnalyzedEmbeddings = {}

        dictAnalyzedEmbeddings[strFile] = lstEmbeddings

        dfAnalyzedEmbeddings = pd.DataFrame.from_dict(dictAnalyzedEmbeddings, orient='index')

        # add the dfAnalyzedEmbeddings to the dfAllEmbeddings dataframe
        dfAllEmbeddings = pd.concat([dfAllEmbeddings, dfAnalyzedEmbeddings])

        # save all embeddings to csv
        dfAllEmbeddings.to_csv(os.path.join(strResultFolder, 'results/embeddings_codex.csv'), sep='$')

        # calculate the distances between the reference and analyzed code
        dfDistances = calculate_distances(dictReferenceEmbeddings, 
                                          dictAnalyzedEmbeddings)

        # print the results
        print_results(dfDistances,strResultFile)