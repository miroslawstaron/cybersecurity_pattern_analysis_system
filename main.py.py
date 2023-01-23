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
from print_header import print_header
from parse_arguments import parse_arguments

# print the welcome message
print_header()

# check if we have two arguments: 
# - -vce the input folder of the reference code
# - -me and the input folder of the code to be analyzed
meFolder, vceFolder = parse_arguments()