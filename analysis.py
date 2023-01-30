############################################################
#
# Secure design pattern and vulnerabilities detection system
#
# (c) Miroslaw Staron, 2021-2023
# www.staron.nu
#
#############################################################

# imports and configuration
import matplotlib.pyplot as plt
import numpy as np
import pandas as pd
import os
from scipy import spatial

# method to calculate the distances between the analyzed code and the reference code
def calculate_distances_ccflex(strEmbeddingsFolder):
    ''' Takes the embeddings matrix as input per module and calculates the distances
        between the analyzed code and the reference code. The output is a dataframe
        with the distances between the analyzed code and the reference code. '''
    
    strDir = strEmbeddingsFolder
    strFile = os.path.join(strDir, 'embeddings.csv')

    print(f'<< Reading the embeddings from the file: {strFile}')

    dfEmbeddings = pd.read_csv(strFile, index_col=0)


    print(f'<< Adding the classes to the embeddings')

    dfEmbeddings['Modified'] = 0

    dfEmbeddings.loc[dfEmbeddings.index.str.startswith('VCE'), 'Modified'] = 2
    dfEmbeddings.loc[dfEmbeddings.index.str.startswith('SCE'), 'Modified'] = 1

    # distance calculation   
    print(f'<< Calculating the distances')
    dictReferenceEmbeddings = dfEmbeddings[dfEmbeddings.Modified > 0].to_dict(orient='index')
    dictCodeEmbeddings = dfEmbeddings[dfEmbeddings.Modified == 0].to_dict(orient='index')

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
            print(f'Distance from {refKeys} to {modKeys}: {distance:.3f}')

    dfDistances = pd.DataFrame.from_records(lstRes, columns=['Reference', 'Module', 'Distance'])
    
    dfDistances.to_csv(os.path.join(strEmbeddingsFolder, 'distances.csv'))

    print(f'<< Done calculating the distances')

    return dfDistances



# method to check if a module is secure or not
def analyze(strEmbeddingsFolder):
    
    ''' Analyzes the code and prints the results. The input is the list of distances
        between the analyzed code and the reference code. The output is the verdict
        whether the analyzed code is secure or vulnerable. 
        The verdict is based on the top 3 closest reference programs'''

    strDir = strEmbeddingsFolder
    strFile = os.path.join(strDir, 'distances.csv')

    print(f'<< Reading the distances from the file: {strFile}')

    dfDistances = pd.read_csv(strFile, index_col=0)

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
            if label.startswith('SCE'):
                iSCEs += 1
            else:
                iVCEs += 1  
    
        # write the verdict
        strVerdict = 'secure' if iSCEs > iVCEs else 'vulnerable'  

        # print the verdict
        print(f'Module {module} is {strVerdict} based on the top 3 closest reference programs')


    return 1