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
        print(f'>> The folder {vceFolder} does not exist')
    if not bME:
        print(f'>> The folder {meFolder} does not exist')
    
    sys.exit(1) if not bVCE or not bME else print('>> Folders exist')

    # check if the folders are not empty
    if len(os.listdir(vceFolder)) == 0:
        print(f'>> The folder {vceFolder} is empty')        
    if len(os.listdir(meFolder)) == 0:
        print(f'>> The folder {meFolder} is empty')
    
    sys.exit(1) if len(os.listdir(vceFolder)) == 0 or len(os.listdir(meFolder)) == 0 else print('>> Folders are empty')
    
    # print the success message
    print('>> Folders exist and are not empty')