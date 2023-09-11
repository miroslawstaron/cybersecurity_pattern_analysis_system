############################################################
#
# Secure design pattern and vulnerabilities detection system
#
# (c) Miroslaw Staron, 2021-2023
# www.staron.nu
#
#############################################################

# import of os
import os

# this function creates a working directory to write the output to
# for now, this is hard coded from the main,
# but the idea is that it will be created based on the arguments
def create_workdir(strDir): 
    print('<< Creating the working directory')
    # create the working directory
    if not os.path.exists(strDir):
        os.makedirs(strDir)
        print(f'>> The folder {strDir} created [OK]')
    else:
        print(f'>> The folder {strDir} exists [!OK]')

    # change the working directory
    os.chdir(strDir)

    # create the subdirectories
    if not os.path.exists('logs'):
        os.makedirs('logs')
        print(f'>> The folder "logs" created [OK]')
    

    if not os.path.exists('results'):
        os.makedirs('results')
        print(f'>> The folder "results" created [OK]')
    

    if not os.path.exists('data'):
        os.makedirs('data')
        print(f'>> The folder "data" created [OK]')
    

    if not os.path.exists('key'):
        os.makedirs('key')
        print(f'>> The folder "key" created [OK]')
    

# copy the folders to the working directory
def copy_folders(strDir, vceFolder, meFolder):
    # copy the folders
    print('<< Copying the folders to the working directory')
    os.system(f'cp -r {vceFolder}/*.* {strDir}/data/vce')
    os.system(f'cp -r {meFolder}/*.* {strDir}/data/me')

    print('>> Folders copied [OK]')