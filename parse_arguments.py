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

# parsing the arguments
def parse_arguments():

    # define the default csv file variable
    csvFile = os.path.join(os.getcwd(), 'workdir/results.csv')

    strModel = 'unknown'

    print(f'<< Parsing the arguments: {len(sys.argv)}')
    # check the number of arguments
    if len(sys.argv) != 7 and len(sys.argv) != 9:
        # if there are three arguments, 
        # check if any of them is a help request
        if len(sys.argv) == 2:
            if sys.argv[1] != '-h' and sys.argv[1] != '--help':
                print("Wrong number of arguments, type -h or --help for help")
                sys.exit(1)
            else:
                print(">>> #######################")
                print("Usage: main.py -vce <reference code folder> -me <code to analyze folder> -m <model (ccflex, codex, singberta, cylbert, codebert)> -csv name.csv")
                print("<<< #######################")
                sys.exit(0)
        # if there are fewer or more than five arguments, 
        # then we write the error message and exit
        else:
            print("Wrong number of arguments, type -h or --help for help")
            sys.exit(1)
    # if there are five arguments,
    # then we check if the first one is -vce and the third one is -me
    else:
        # parser arguments
        for i in range(1, len(sys.argv)):
            if sys.argv[i] == '-vce':
                vceFolder = sys.argv[i+1]
            if sys.argv[i] == '-me':
                meFolder = sys.argv[i+1]
            if sys.argv[i] == '-m':
                strModel = sys.argv[i+1]
                if not strModel in ['ccflex', 'codex', 'singberta', 'cylbert' , 'codebert']:
                    print("Wrong model, type -h or --help for help")
                    sys.exit(1)
            if sys.argv[i] == '-csv':
                csvFile = sys.argv[i+1]

        # print the configuration arguments
        print(f'>> VCE folder is: {vceFolder}')
        print(f'>> ME folder is: {meFolder}')
        print(f'>> Model is: {strModel}')
        print(f'>> CSV file is: {csvFile}')
    
    # return the parsed folders
    return vceFolder, meFolder, strModel, csvFile