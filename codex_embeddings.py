#!/bin/python3
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

# this function retrieves embeddings from CodeX
# it is taken as-is from the Embeddings API documentation
def get_embedding(text, engine="code-search-babbage-code-001"):
   text = text.replace("\n", " ")
   return openai.Embedding.create(input = [text], engine=engine)['data'][0]['embedding']


def extract_embeddings_codex(strEmbeddingsFolder, 
                             strCodeFolder, 
                             strReferenceCodeFolder):
    with open('./openAI_key.txt', 'r') as fKey:
        theKey = fKey.readline()

    dictEmbeddingsCode = {}
    dictEmbeddingsReferenceCode = {}
    dictDF = {}

    # Load your API key from an environment variable or secret management service
    openai.api_key = theKey

    # get the list of files from the strCodeFolder
    # and iterate over them

    print('<< Extracting embeddings from CodeX, code')

    for strFile in os.listdir(strCodeFolder):
        # get the full path to the file
        strFullPath = os.path.join(strCodeFolder, strFile)
        # read the file
        with open(strFullPath, 'r') as fCode:
            strCode = fCode.read()
            # get the embedding
            strEmbedding = get_embedding(strCode)
            # add the embedding to the dictionary
            dictEmbeddingsCode[strFile] = strEmbedding

    print('<< Extracting embeddings from CodeX... done')

    print('<< Extracting embeddings from CodeX, reference code')

    # now the same for the reference code
    for strFile in os.listdir(strReferenceCodeFolder):
        # get the full path to the file
        strFullPath = os.path.join(strReferenceCodeFolder, strFile)
        # read the file
        with open(strFullPath, 'r') as fCode:
            strCode = fCode.read()
            # get the embedding
            strEmbedding = get_embedding(strCode)
            # add the embedding to the dictionary
            dictEmbeddingsReferenceCode[strFile] = strEmbedding

    print('<< Extracting embeddings from CodeX... done')

    print('<< Saving embeddings to a file')

    iIndex = 0

    for reqID in dictEmbeddingsCode.keys():
        dictDF[reqID] = [0, dictEmbeddingsCode[reqID]]
        iIndex += 1

    for key in dictEmbeddingsReferenceCode.keys():
        dictDF[key] = [1, dictEmbeddingsReferenceCode[key]]
        iIndex += 1
    
    # convert the dictionary to a dataframe
    dfEmbeddings = pd.DataFrame.from_dict(dictDF, 
                                          orient='index',
                                          columns=['modified', 'embeddings'])

    # save the dataframe to a file
    dfEmbeddings.to_csv(os.path.join(strEmbeddingsFolder, 'embeddings.csv'), sep=';')

        