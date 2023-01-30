#!/bin/python3
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
from analysis import analyze                            # analyzes the code
from analysis import calculate_distances_ccflex         # calculates the distances between the analyzed code and the reference code

# debug flag, to steer what we actually calculate
bDebug = True


# configuration parameters
# working directory
strWorkDir = '/mnt/c/Users/miros/Documents/Code/cybersecurity_pattern_analysis_system/workdir'                                  

# print the welcome message
print_header()

# check if we have two arguments: 
# - -vce the input folder of the reference code
# - -me and the input folder of the code to be analyzed
# 
# if the folders are empty, then we exit
# and that is handled by the parse_arguments function
if not bDebug:
    meFolder, vceFolder = parse_arguments()

# check if the folders exist and if they are not empty
if not bDebug:
    check_folders(meFolder, vceFolder)

# create the working directory
if not bDebug:
    create_workdir(strWorkDir)

# copy the folders to the working directory
if not bDebug:
    copy_folders(strWorkDir, meFolder, vceFolder)

# create the feature vectors for the analyzed code

# calculate the similarity between the analyzed code and the reference code
calculate_distances_ccflex(os.path.join(strWorkDir, 'results'))

# visualize the results and provide the user with the analysis' results
analyze(os.path.join(strWorkDir, 'results'))

# end the program, print the end message
print_end()