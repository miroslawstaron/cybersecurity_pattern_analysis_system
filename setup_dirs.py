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
    else:
        print(f'>> The folder "logs" exists [!OK]')

    if not os.path.exists('results'):
        os.makedirs('results')
        print(f'>> The folder "results" created [OK]')
    else:
        print(f'>> The folder "results" exists [!OK]')

    if not os.path.exists('data'):
        os.makedirs('data')
        print(f'>> The folder "data" created [OK]')
    else:
        print(f'>> The folder "data" exists [!OK]')

    if not os.path.exists('data/vce'):
        os.makedirs('data/vce')
        print(f'>> The folder "data/vce" created [OK]')
    else:
        print(f'>> The folder "data/vce" exists [!OK]')

    if not os.path.exists('data/me'):
        os.makedirs('data/me')
        print(f'>> The folder "data/me" created [OK]')
    else:
        print(f'>> The folder "data/me" exists [!OK]')


    if not os.path.exists('data/vce/feature_vectors'):
        os.makedirs('data/vce/feature_vectors')
        print(f'>> The folder "data/vce/feature_vectors" created [OK]')
    else:
        print(f'>> The folder "data/vce/feature_vectors" exists [!OK]')
        
    if not os.path.exists('data/me/feature_vectors'):
        os.makedirs('data/me/feature_vectors')
        print(f'>> The folder "data/me/feature_vectors" created [OK]')
    else:
        print(f'>> The folder "data/me/feature_vectors" exists [!OK]')


# copy the folders to the working directory
def copy_folders(strDir, vceFolder, meFolder):
    # copy the folders
    print('<< Copying the folders to the working directory')
    os.system(f'cp -r {vceFolder}/*.* {strDir}/data/vce')
    os.system(f'cp -r {meFolder}/*.* {strDir}/data/me')

    print('>> Folders copied [OK]')