#!/bin/python3
#############################################################
#
# Secure design pattern and vulnerabilities detection system
#
# (c) Miroslaw Staron, 2021-2023
# www.staron.nu
#
#############################################################

# This module uses CylBERTa to extract the embeddings from the code
# the model is trained on the code of WolfSSL and examples of input validation vulnerabilities
# 
from transformers import RobertaTokenizer
from transformers import RobertaConfig
from transformers import pipeline
import numpy as np
import pandas as pd
import os
from scipy import spatial

def extract_embeddings_cylbert(strFolder, 
                                 strCodeFolder, 
                                 strReferenceFolder):

    config = RobertaConfig(
        vocab_size=52_000,
        max_position_embeddings=514,
        num_attention_heads=12,
        num_hidden_layers=6,
        type_vocab_size=1,
    )

    
    tokenizer = RobertaTokenizer.from_pretrained("mstaron/CyBERTa", max_length=512)

    # create the pipeline, which will extract the embedding vectors
    # the models are already pre-defined, so we do not need to train anything here
    features = pipeline(
        "feature-extraction",
        model="mstaron/CyBERTa",
        tokenizer="mstaron/CyBERTa", 
        return_tensor = False
    )

    # get all files in the folder
    lstFiles = os.listdir(strCodeFolder)

    lstFullPaths = []

    for strFile in lstFiles:
        lstFullPaths.append(os.path.join(strCodeFolder, strFile))

    lstReference = os.listdir(strReferenceFolder)

    for strFile in lstReference:
        lstFullPaths.append(os.path.join(strReferenceFolder, strFile))

    # now go through all the files and extract embeddings
    dictEmbeddingsFiles = {}

    # counter of the files
    iFiles = 0

    for strFile in lstFullPaths:
        print(f'Processing file {iFiles+1} of {len(lstFullPaths)} files')
        iFiles += 1

        # read the file from the data directory
        with open(strFile, 'r') as f:
            lstLines = f.readlines()

        # now go through all the lines and extract embeddings
        dictEmbeddings = {}

        # counter of the lines
        iLines = 0

        for strLine in lstLines:

            # print the progress
            iLines += 1
            if iLines % 1000 == 0:
                print(f'Processed {iLines} lines of {len(lstLines)} of file {iFiles} of {len(lstFiles)} files')

            # extract the features == embeddings
            lstFeatures = features(strLine)

            # get the embedding of the first token [CLS]
            # which is also a good approximation of the whole sentence embedding
            # the same as using np.mean(lstFeatures[0], axis=0)
            lstEmbedding = lstFeatures[0][0]

            # store the embedding in the dictionary
            dictEmbeddings[strLine] = lstEmbedding
        
        dfEmbeddings = pd.DataFrame.from_dict(dictEmbeddings, orient='index')
        lstEmbedding = np.mean(dfEmbeddings.values, axis=0)
        dictEmbeddingsFiles[strFile] = lstEmbedding

    dfEmbeddingFile = pd.DataFrame.from_dict(dictEmbeddingsFiles, orient='index')

    # save the embeddings to a file
    dfEmbeddingFile.to_csv(os.path.join(strFolder, './embeddings_cylberta.csv'), sep='$')

    return dfEmbeddingFile

def calculate_distances_cylbert(strEmbeddingsFolder):
    ''' Takes the embeddings matrix as input per module and calculates the distances
        between the analyzed code and the reference code. The output is a dataframe
        with the distances between the analyzed code and the reference code. '''
    
    strDir = strEmbeddingsFolder
    strFile = os.path.join(strDir, 'embeddings_cylberta.csv')

    print(f'<< Reading the embeddings from the file: {strFile}')

    dfEmbeddings = pd.read_csv(strFile, index_col=0, sep='$')

    print(dfEmbeddings.shape)

    print(f'<< Adding the classes to the embeddings')

    dfEmbeddings['Modified'] = 0

    dfEmbeddings.loc[dfEmbeddings.index.str.contains('VCE_'), 'Modified'] = 2
    dfEmbeddings.loc[dfEmbeddings.index.str.contains('SCE_'), 'Modified'] = 1

    # distance calculation   
    print(f'<< Calculating the distances')

    dictReferenceEmbeddings = dfEmbeddings[dfEmbeddings.Modified > 0].drop(['Modified'], axis=1).to_dict(orient='index')
    dictCodeEmbeddings = dfEmbeddings[dfEmbeddings.Modified == 0].drop(['Modified'], axis=1).to_dict(orient='index')

    # print(dictCodeEmbeddings)

    dictCodeEmbeddingsL = {}
    dictReferenceEmbeddingsL ={}

    # dict values to list
    for oneEl in dictCodeEmbeddings.keys():
        dictCodeEmbeddingsL[oneEl] = list(dictCodeEmbeddings[oneEl].values())

    # and the same for reference embeddings
    for oneEl in dictReferenceEmbeddings.keys():
        dictReferenceEmbeddingsL[oneEl] = list(dictReferenceEmbeddings[oneEl].values())
        

    # list of lists with the results
    lstRes = []

    for refKeys in dictReferenceEmbeddingsL.keys():
        for modKeys in dictCodeEmbeddingsL.keys():
            distance = spatial.distance.cosine(dictReferenceEmbeddingsL[refKeys], dictCodeEmbeddingsL[modKeys])
            oneDistance = [refKeys, modKeys, distance]
            lstRes.append(oneDistance)
            #print(f'Distance from {refKeys} to {modKeys}: {distance:.3f}')

    dfDistances = pd.DataFrame.from_records(lstRes, columns=['Reference', 'Module', 'Distance'])
    
    dfDistances.to_csv(os.path.join(strEmbeddingsFolder, 'distances_cylberta.csv'))

    print(f'<< Done calculating the distances')

    return dfDistances

# method to check if a module is secure or not
def analyze_cylbert(strEmbeddingsFolder):
    
    ''' Analyzes the code and prints the results. The input is the list of distances
        between the analyzed code and the reference code. The output is the verdict
        whether the analyzed code is secure or vulnerable. 
        The verdict is based on the top 3 closest reference programs'''

    strDir = strEmbeddingsFolder
    strFile = os.path.join(strDir, 'distances_cylberta.csv')

    print(f'<< Reading the distances from the file: {strFile}')

    dfDistances = pd.read_csv(strFile, index_col=0, sep=',')

    # for each module, visualize the distances 
    # between that module and the reference code samples

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
        dfModuleLabels = dfModule['Reference']

        # print(dfModuleLabels)

        # check if they are SCEs or VCEs
        iSCEs = 0
        iVCEs = 0
        for label in dfModuleLabels:
            if 'SCE_' in label:
                iSCEs += 1
            else:
                iVCEs += 1  
    
        # write the verdict
        strVerdict = 'secure' if iVCEs > iSCEs else 'vulnerable'  

        mName = module.split('/')[-1]

        # print the verdict
        # changed to printing only the violations and then the examples
        if strVerdict == 'vulnerable':
            print('*****')
            print(f'Module {mName} is flagged as {strVerdict}, with the following reference violations:')

            # here we print only the vulnerable modules and not all three
            # in order to not confuse the user
            for label in dfModuleLabels:
                if 'VCE_' in label:
                    print(f'>>>> {label.split("/")[-1]}')

    return 1
