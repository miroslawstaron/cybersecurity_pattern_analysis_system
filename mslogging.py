############################################################
#
# Secure design pattern and vulnerabilities detection system
#
# (c) Miroslaw Staron, 2021-2023
# www.staron.nu
#
#############################################################

import logging

# function to setup the logging
def setup_logging():
    logging.basicConfig(filename='./logfile.log', 
                        filemode='w',
                        format='%(asctime)s;%(name)s;%(levelname)s;%(message)s',
                        level=logging.DEBUG)

    global logger
    logger = logging.getLogger('Cybersecurity analyzer')

    logger.info('Configuration started')

    return logger