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

# parsing the arguments
def parse_arguments():
    print('<< Parsing the arguments')
    # check the number of arguments
    if len(sys.argv) != 5:
        # if there are two arguments, 
        # check if any of them is a help request
        if len(sys.argv) == 2:
            if sys.argv[1] != '-h' and sys.argv[1] != '--help':
                print("Wrong number of arguments, type -h or --help for help")
                sys.exit(1)
            else:
                print("Usage: main.py -vce <reference code folder> -me <code to analyze folder>")
                sys.exit(0)
        # if there are fewer or more than five arguments, 
        # then we write the error message and exit
        else:
            print("Wrong number of arguments, type -h or --help for help")
            sys.exit(1)
    # if there are five arguments,
    # then we check if the first one is -vce and the third one is -me
    else:
        # check if the 1st argument is me or vce
        # and then assign the variables accordingly
        if sys.argv[1] == '-vce':
            vceFolder = sys.argv[2]
            meFolder = sys.argv[4]
        if sys.argv[1] == '-me':
            vceFolder = sys.argv[4]
            meFolder = sys.argv[2]
        
        # print the configuration arguments
        print(f'>> VCE folder is: {vceFolder}')
        print(f'>> ME folder is: {meFolder}')
    
    # return the parsed folders
    return vceFolder, meFolder