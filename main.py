#############################################################
#
# Secure design pattern and vulnerabilities detection system
#
# (c) Miroslaw Staron, 2021-2023
# www.staron.nu
#
#############################################################

# This is the main file of the system. It is responsible for the following:
# parsing the command line arguments
# reading the configuration file
# coordinating the algorithm execution

# import libraries needed for the system
import sys                                              # system library
import logging                                          # logging library
import os
from print_header import print_header, print_end        # prints the header and the end message
from parse_arguments import parse_arguments             # parses the arguments
from checks import check_folders                        # checks if the folders exist and are not empty
from setup_dirs import create_workdir                   # creates the working directory
from setup_dirs import copy_folders                     # copies the folders to the working directory
from analysis import analyze_ccflex                     # analyzes the code
from analysis import analyze_codex                      # analyzes the code
from analysis import calculate_distances_ccflex         # calculates the distances between the analyzed code and the reference code
from ccflex_embeddings import extract_embeddings_ccflex # extracts the embeddings matrix from the analyzed code
from analysis import create_average_embeddings_ccflex   # creates the average embeddings matrix from the embeddings matrix
from codex_embeddings import extract_embeddings_codex   # extracts the embeddings matrix from the analyzed code
from analysis import calculate_distances_codex          # calculates the distances between the analyzed code and the reference code
import time                                             # time library for sleep, only for the demo
from singberta_embeddings import extract_embeddings_singberta       # extracts the embeddings matrix from the analyzed code using the SingBERTa model
from singberta_embeddings import calculate_distances_singberta      # calculates the distances between the analyzed code and the reference code using the SingBERTa model
from singberta_embeddings import analyze_singberta      # analyzes the code using the SingBERTa model
import cylberta_embeddings
import codebert_embeddings
import analysis
import single_file_pipeline

# disable all warnings
import warnings
warnings.filterwarnings("ignore")

import logging
logging.captureWarnings(True)

def cylbert_analyze():
    ''' 
    This function is used to analyze the code using the CylBERT model.
    '''
    cylberta_embeddings.extract_embeddings_cylbert(os.path.join(strWorkDir, 'results'),
                                vceFolder, 
                                meFolder)

    # calculate the similarity between the analyzed code and the reference code
    cylberta_embeddings.calculate_distances_cylbert(os.path.join(strWorkDir, 'results'))

    # analyze the distances and print the code
    cylberta_embeddings.analyze_cylbert(os.path.join(strWorkDir, 'results'), 
                                        csvFile)

def codebert_analyze():
    ''' 
    This function is used to analyze the code using the CylBERT model.
    '''
    codebert_embeddings.extract_embeddings_codebert(os.path.join(strWorkDir, 'results'),
                                vceFolder, 
                                meFolder)

    # calculate the similarity between the analyzed code and the reference code
    analysis.calculate_distances(os.path.join(strWorkDir, 'results/embeddings_codebert.csv'), 
                                 os.path.join(strWorkDir, 'results'))

    # analyze the distances and print the code
    analysis.analyze(os.path.join(strWorkDir, 'results/distances_codebert.csv'),
                    csvFile)
    
# debug flag, to steer what we actually calculate
bDebug = False

# configuration parameters
# working directory
currentDir = os.getcwd()
strWorkDir = os.path.join(currentDir, 'workdir')

# print the welcome message
print_header()

# check if we have two arguments: 
# - -vce the input folder of the reference code
# - -me and the input folder of the code to be analyzed
# 
# if the folders are empty, then we exit
# and that is handled by the parse_arguments function
vceFolder, meFolder, strModel, csvFile = parse_arguments()

# check if the folders exist and if they are not empty
check_folders(meFolder, vceFolder)

# create the working directory
create_workdir(strWorkDir)

# extract embeddings from CodeX
if strModel == 'codex':
    single_file_pipeline.pipeline_codex(vceFolder,
                                meFolder,
                                os.path.join(strWorkDir, 'results/result_codex.csv'),
                                strWorkDir,  
                                os.path.join(strWorkDir, 'key/openAI_key.txt'))
    
    #extract_embeddings_codex(os.path.join(strWorkDir, 'results'), 
    #                         vceFolder,
    #                         meFolder, 
    #                         os.path.join(strWorkDir, 'key/openAI_key.txt'))
                             
    # calculate the similarity between the analyzed code and the reference code
    #calculate_distances_codex(os.path.join(strWorkDir, 'results'))

    # analyze the distances and print the code
    #analyze_codex(os.path.join(strWorkDir, 'results'), csvFile)

# create the feature vectors for the analyzed code
if strModel == 'ccflex':

    extract_embeddings_ccflex(os.path.join(strWorkDir, 'results'), 
                              vceFolder, 
                              meFolder, 
                              manual_vocab = ['main', 'string', 'int'],
                              bow_vocab_size = 100)
    
    # CCFlex creates embeddings for each line, so we need to create the average embeddings
    create_average_embeddings_ccflex(os.path.join(strWorkDir, 'results'))

    # calculate the similarity between the analyzed code and the reference code
    calculate_distances_ccflex(os.path.join(strWorkDir, 'results'))
    # analyze the distances and print the code
    analyze_ccflex(os.path.join(strWorkDir, 'results'))

if strModel == 'singberta':
    single_file_pipeline.pipeline(vceFolder,
                                meFolder,
                                os.path.join(strWorkDir, 'results/result.csv'),
                                strWorkDir, 
                                'singberta')

if strModel == 'cylbert':
    #cylbert_analyze()
    single_file_pipeline.pipeline(vceFolder,
                                meFolder,
                                os.path.join(strWorkDir, 'results/result.csv'), 
                                strWorkDir,
                                'cylbert')

if strModel == 'codebert':
    #codebert_analyze()
    single_file_pipeline.pipeline(vceFolder,
                                meFolder,
                                os.path.join(strWorkDir, 'results/result.csv'), 
                                strWorkDir,
                                'codebert')
    
time.sleep(3)

# end the program, print the end message
print_end()