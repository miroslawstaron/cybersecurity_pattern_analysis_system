############################################################
#
# Secure design pattern and vulnerabilities detection system
#
# (c) Miroslaw Staron, 2021-2023
# www.staron.nu
#
#############################################################

# imports
import sys
import os
import mslogging

def add_two_numbers(a, b):
    return a+b

# checking if folders exist and are not empty
def check_folders(vceFolder, meFolder):
    bVCE = True
    bME = True

    print('<< Checking if folders exist and are not empty')
    
    # check if the folders exist
    if not os.path.exists(vceFolder) or not os.path.isdir(vceFolder):
        bVCE = False
    if not os.path.exists(meFolder) or not os.path.isdir(meFolder):
        bME = False
    
    if not bVCE:
        print(f'>> The folder {vceFolder} does not exist [!OK]')
    
    if not bME:
        print(f'>> The folder {meFolder} does not exist [!OK]')
    
    
    sys.exit(1) if not bVCE or not bME else print('>> Folders exist [OK]')

    # check if the folders are not empty
    if len(os.listdir(vceFolder)) == 0:
        print(f'>> The folder {vceFolder} is empty [!OK]')        
    
    if len(os.listdir(meFolder)) == 0:
        print(f'>> The folder {meFolder} is empty [!OK]')
    
    
    sys.exit(1) if len(os.listdir(vceFolder)) == 0 or len(os.listdir(meFolder)) == 0 else print('>> Folders are not empty [OK]')
    
