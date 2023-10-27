#############################################################
#
# Secure design pattern and vulnerabilities detection system
#
# (c) Miroslaw Staron, 2021-2023
# www.staron.nu
#
#############################################################

#
# This file includes functions to extract embeddings from any model from HuggingFace
# The model needs to have the "feature-extraction" pipeline
# and needs to be publicly available

# import libraries needed for the system
from transformers import RobertaTokenizer
from transformers import RobertaConfig
from transformers import pipeline
import numpy as np
import pandas as pd
import os
from scipy import spatial

# extract embeddings from one file only
def extract_embeddings_codebert_one_file(strFile):
    tokenizer = RobertaTokenizer.from_pretrained("microsoft/codebert-base", max_length=512)

    # create the pipeline, which will extract the embedding vectors
    # the models are already pre-defined, so we do not need to train anything here
    features = pipeline(
        "feature-extraction",
        model="microsoft/codebert-base",
        tokenizer="microsoft/codebert-base", 
        return_tensor = False
    )

    # read the file from the data directory
    with open(strFile, 'r', encoding='utf-8', errors='ignore') as f:
        lstLines = f.readlines()
    
    
    # now go through all the lines and extract embeddings
    dictEmbeddings = {}

    # counter of the lines
    iLines = 0

    for strLine in lstLines:

        # print the progress
        iLines += 1
        if iLines % 1000 == 0:
            print(f'Processed {iLines} lines of {len(lstLines)} of file {strFile}')

        try:
            # extract the features == embeddings
            lstFeatures = features(strLine)

            # get the embedding of the first token [CLS]
            # which is also a good approximation of the whole sentence embedding
            # the same as using np.mean(lstFeatures[0], axis=0)
            lstEmbedding = lstFeatures[0][0]
    
            # store the embedding in the dictionary
            dictEmbeddings[strLine] = lstEmbedding
        except:
            print(f'Problem in embedding line, skipping {strLine[:5]}...')
    
    dfEmbeddings = pd.DataFrame.from_dict(dictEmbeddings, orient='index')
    lstEmbeddings = np.mean(dfEmbeddings.values, axis=0)

    # we return the list of embeddings for the file
    return lstEmbeddings

# extract embeddings from the entire folder structure
def extract_embeddings_codebert_dict(strFolder):

    tokenizer = RobertaTokenizer.from_pretrained("microsoft/codebert-base", max_length=512)

    # create the pipeline, which will extract the embedding vectors
    # the models are already pre-defined, so we do not need to train anything here
    features = pipeline(
        "feature-extraction",
        model="microsoft/codebert-base",
        tokenizer="microsoft/codebert-base", 
        return_tensor = False
    )
    
    lstFullPaths = []

    for root, dirs, files in os.walk(strFolder):
        for strFile in files:
            lstFullPaths.append(os.path.join(root, strFile))

    # now go through all the files and extract embeddings
    dictEmbeddingsFiles = {}

    # counter of the files
    iFiles = 0

    for strFile in lstFullPaths:
        print(f'Processing reference file {iFiles+1} of {len(lstFullPaths)} files')
        iFiles += 1

        # read the file from the data directory
        if os.path.isfile(strFile):
            with open(strFile, 'r', encoding='utf-8', errors='ignore') as f:
                lstLines = f.readlines()

            # now go through all the lines and extract embeddings
            dictEmbeddings = {}

            # counter of the lines
            iLines = 0

            for strLine in lstLines:

                # print the progress
                iLines += 1
                if iLines % 1000 == 0:
                    print(f'Processed {iLines} lines of {len(lstLines)} of file {iFiles} of {len(lstFullPaths)} files')

                # extract the features == embeddings
                # check if the line, after tokenization, is less than 512 tokens
                # if it is, then we can extract the embeddings
                try:
                    lstFeatures = features(strLine)

                    # get the embedding of the first token [CLS]
                    # which is also a good approximation of the whole sentence embedding
                    # the same as using np.mean(lstFeatures[0], axis=0)
                    lstEmbedding = lstFeatures[0][0]

                    # store the embedding in the dictionary
                    dictEmbeddings[strLine] = lstEmbedding
                except:
                    print(f'Problem in embedding line, skipping: {strLine[:5]}...')

            dfEmbeddings = pd.DataFrame.from_dict(dictEmbeddings, orient='index')
            lstEmbedding = np.mean(dfEmbeddings.values, axis=0)
            dictEmbeddingsFiles[strFile] = lstEmbedding

    return dictEmbeddingsFiles

def extract_embeddings_codebert(strFolder, 
                                strCodeFolder, 
                                strReferenceFolder):

    tokenizer = RobertaTokenizer.from_pretrained("microsoft/codebert-base", max_length=512)

    # create the pipeline, which will extract the embedding vectors
    # the models are already pre-defined, so we do not need to train anything here
    features = pipeline(
        "feature-extraction",
        model="microsoft/codebert-base",
        tokenizer="microsoft/codebert-base", 
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
    dfEmbeddingFile.to_csv(os.path.join(strFolder, './embeddings_codebert.csv'), sep='$')

    return dfEmbeddingFile
