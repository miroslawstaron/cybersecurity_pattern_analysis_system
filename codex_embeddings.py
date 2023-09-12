#############################################################
#
# Secure design pattern and vulnerabilities detection system
#
# (c) Miroslaw Staron, 2021-2023
# www.staron.nu
#
#############################################################
import os
import openai
import pandas as pd
import numpy as np

# this function retrieves embeddings from CodeX
# it is taken as-is from the Embeddings API documentation
def get_embedding(text, engine="code-search-babbage-code-001", theKey=None):
   text = text.replace("\n", " ")
   openai.api_key = theKey
   emb = openai.Embedding.create(input = [text], engine=engine)['data'][0]['embedding']
   return emb

# extract embeddings from one file only
def extract_embeddings_codex_one_file(strFile, strCodeXKeyFile):
    with open(strCodeXKeyFile, 'r') as fKey:
        theKey = fKey.readline()

    # Load your API key from an environment variable or secret management service
    openai.api_key = theKey

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
            print(f'Processed {iLines} lines of {len(lstLines)} of file {strFile}')

        # extract the features == embeddings
        lstEmbedding = get_embedding(strLine, theKey=theKey)

        # store the embedding in the dictionary
        dictEmbeddings[strLine] = lstEmbedding

    dfEmbeddings = pd.DataFrame.from_dict(dictEmbeddings, orient='index')
    lstEmbeddings = np.mean(dfEmbeddings.values, axis=0)

    # we return the list of embeddings for the file
    return lstEmbeddings

# extract embeddings from the entire folder structure
def extract_embeddings_codex_dict(strFolder,strCodeXKeyFile):

    with open(strCodeXKeyFile, 'r') as fKey:
        theKey = fKey.readline()

    # Load your API key from an environment variable or secret management service
    openai.api_key = theKey
    
    lstFullPaths = []

    if strFolder != '':
        lstReference = os.listdir(strFolder)
    else:
        lstReference = []

    for strFile in lstReference:
        lstFullPaths.append(os.path.join(strFolder, strFile))

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
                print(f'Processed {iLines} lines of {len(lstLines)} of file {strFile}')

            # extract the features == embeddings
            lstEmbedding = get_embedding(strLine, theKey=theKey)

            # store the embedding in the dictionary
            dictEmbeddings[strLine] = lstEmbedding
        
        dfEmbeddings = pd.DataFrame.from_dict(dictEmbeddings, orient='index')
        lstEmbedding = np.mean(dfEmbeddings.values, axis=0)
        dictEmbeddingsFiles[strFile] = lstEmbedding

    return dictEmbeddingsFiles

def extract_embeddings_codex(strEmbeddingsFolder,                              
                             strReferenceCodeFolder, 
                             strCodeFolder, 
                             strCodeXKeyFile):
    with open(strCodeXKeyFile, 'r') as fKey:
        theKey = fKey.readline()

    dictEmbeddingsFiles = {}

    # Load your API key from an environment variable or secret management service
    openai.api_key = theKey

    # get the list of files from the strCodeFolder
    # and iterate over them
    # get all files in the folder
    lstFiles = os.listdir(strCodeFolder)

    lstFullPaths = []

    for strFile in lstFiles:
        lstFullPaths.append(os.path.join(strCodeFolder, strFile))

    lstReference = os.listdir(strReferenceCodeFolder)

    for strFile in lstReference:
        lstFullPaths.append(os.path.join(strReferenceCodeFolder, strFile))

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
            lstEmbedding = get_embedding(strLine, theKey=theKey)

            # store the embedding in the dictionary
            dictEmbeddings[strLine] = lstEmbedding
        
        dfEmbeddings = pd.DataFrame.from_dict(dictEmbeddings, orient='index')
        lstEmbedding = np.mean(dfEmbeddings.values, axis=0)
        dictEmbeddingsFiles[strFile] = lstEmbedding

    
    print('<< Extracting embeddings from CodeX... done')

    print('<< Saving embeddings to a file')

    dfEmbeddingFile = pd.DataFrame.from_dict(dictEmbeddingsFiles, orient='index')

    print("Shape of the embeddings file")
    print(dfEmbeddingFile.shape)

    # save the embeddings to a file
    strFileName = os.path.join(strEmbeddingsFolder, 'embeddings_codex.csv')
    print(f'Saving embeddings to {strFileName}')

    dfEmbeddingFile.to_csv(strFileName, sep='$')

    return dfEmbeddingFile

        