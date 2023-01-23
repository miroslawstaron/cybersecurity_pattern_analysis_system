############################################################
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
import sys
from print_header import print_header, print_end
from parse_arguments import parse_arguments
from checks import check_folders

# print the welcome message
print_header()

# check if we have two arguments: 
# - -vce the input folder of the reference code
# - -me and the input folder of the code to be analyzed
# 
# if the folders are empty, then we exit
# and that is handled by the parse_arguments function
meFolder, vceFolder = parse_arguments()

# check if the folders exist and if they are not empty
check_folders(meFolder, vceFolder)

# create the feature vectors for the analyzed code

# calculate the similarity between the analyzed code and the reference code

# visualize the results and provide the user with the analysis' results

# end the program, print the end message
print_end()