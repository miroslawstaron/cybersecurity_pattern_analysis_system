# Cybersecurity Pattern and Vulnerability Pattern detection

This program is a pattern detection program that can detect patterns in a text file. It can detect both cybersecurity patterns and vulnerability patterns. 

## Usage
To use the program, you need to provide the program with two arguments: 
* -vce: The path to the vulnerability pattern file
* -me: The path to the code to analyze
* -m: model used to find similarities - codex or ccflex

Typing '-h' or '--help' will display the help menu.

## Basics
The software uses programming language models to transform each module into an embedding vector and compares the vectors. The comparison is done based on the distances between the modules and the reference code. 

The reference code is collected manually as part of a research project and illustrate examples of vulnerabilities as well as their solutions. 

## Repository structure:
* main.py - the main module to execute with the parameters
* analysis.py - module that analyzes the distances and find the most similar programs
* checks.py - module that checks if the environment is set in the correct way
* codex_embeddings.py - module to extract the embeddings from CodeX using the openAI API
* mslogging.py - logging module (not used at the moment)
* parse_arguments.py - module to parse the arguments from the command line
* print_header.py - module to print the header and footer of the program
* setup_dirs.py - module that sets up the directories for the product

## Authors
Miroslaw Staron, [email](mailto:miroslaw.staron@gu.se)
(C) Miroslaw Staron, 2021-2023
