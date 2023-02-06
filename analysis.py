############################################################
#
# Secure design pattern and vulnerabilities detection system
#
# (c) Miroslaw Staron, 2021-2023
# www.staron.nu
#
#############################################################

# imports and configuration
import matplotlib.pyplot as plt
import numpy as np
import pandas as pd
import os
from scipy import spatial
from prepare.case_extractors import LinesCaseExtractor
from prepare.vocabularies import VocabularyExtractor, code_stop_words_tokenizer, token_signature
from sklearn.feature_extraction.text import CountVectorizer
from prepare.feature_extractors import SubstringCountingFeatureExtraction, WholeWordCountingFeatureExtraction, \
    LineFeaturesExtractionController, CommentFeatureExtraction, WordCountFeatureExtraction, CharCountFeatureExtraction, \
    WholeLineCommentFeatureExtraction, BlankLineFeatureExtraction, PythonWholeLineCommentFeatureExtraction, \
    RegexpCountingFeatureExtraction, TokenizedWholeWordCountingFeatureExtraction

from prepare.feature_extractors import CountVectorizerBasedFeatureExtraction

import json
import csv
import time
import gc

# method to extract embeddings matrix
def extract_embeddings_ccflex(strEmbeddingsFolder, strCodeFolder):
    ''' Extracts the embeddings matrix from the analyzed code. The input is the path
        to the analyzed code. The output is the embeddings matrix. '''

    # this is a bit of an ugly hack
    # where I do not really build the config file
    # but just reuse the one from the original ccflex folder
    # but hey, if it works, it works
    strClasses = '''{
        "labeled": [
        {
            "line_prefix": "@",
            "name": "count",
            "value": 1
        }
        ],
        "default": {
        "name": "ignore",
        "value": 0
        }
    }'''

    strLocations = '''
    {
    "train": {
        "baseline_dir": "./workspace",
        "locations": [
        {
            "path": "./workspace",
            "include": [
            ".+[.]cpp$",
            ".+[.]cs$",
            ".+[.]c$",
            ".+[.]h$"
            ],
            "exclude": []
        },
        {
            "path": "./workspace",
            "include": [
            ".+[.]cpp$",
            ".+[.]cs$",
            ".+[.]c$",
            ".+[.]h$"
            ],
            "exclude": []
        }
        ]
    },

    "workspace_dir": {
        "path": "./ccflex_tmp",
        "erase": false
    },

    "rscript_executable_path": "C:/Program Files/R/bin/RScript.exe" 
    }
    '''
    separator = ';'
    top_words_threshold = 10000
    include_statistics = True
    token_signature_for_missing = True
    min_ngrams = 1
    max_ngrams = 2

    dictLocations = json.loads(strLocations)
    dictLocations['train']['workspace_dir'] = '/mnt/c/users/miros/documents/Code/cybersecurity_code_database/cs_input_validation/evaluation'
    dictLocations['train']['locations'][0]['path'] = '/mnt/c/users/miros/documents/Code/cybersecurity_code_database/cs_input_validation/evaluation'
    dictLocations['train']['locations'][1]['path'] = '/mnt/c/users/miros/documents/Code/cybersecurity_code_database/cs_input_validation/reference'

    # print(dictLocations['train'])

    print(f'<< Converting lines to csv') 
    # convert the string to a dictionary
    # so that we can use it in the LinesCaseExtractor
    dictClasses = json.loads(strClasses)

    outputPath = os.path.join(strEmbeddingsFolder, 'lines.csv')

    lines_extractor = LinesCaseExtractor(code_location = dictLocations['train'], 
                                        output_file_path = outputPath, 
                                        decision_classes = dictClasses,
                                        sep=';',
                                        quotechar="\"",
                                        remove_duplicates=True)

    lines_extractor.extract()

    print(f'<< Done converting lines to csv')
    
    print(f'<< Vocabulary extraction')
    vocab_file_name = 'vocab.csv'
    workspace_dir = strEmbeddingsFolder
    vocabFile = os.path.join(strEmbeddingsFolder, vocab_file_name)
    base_output_file_path = os.path.join(strEmbeddingsFolder, "base-" + vocab_file_name)
    base_output_json_file_path = os.path.join(strEmbeddingsFolder, "base-" + vocab_file_name.replace(".csv", ".json"))

    input_file_path = outputPath
    vocab_extractor = VocabularyExtractor(input_file_path, separator)
    vocab_extractor.extract()

    base_vocab = vocab_extractor.vocab
    
    if top_words_threshold > 0:
        print(">>>> Taking {} most frequent words".format(top_words_threshold))
        base_vocab = base_vocab.head(top_words_threshold)

    if include_statistics:
        base_vocab.to_csv(base_output_file_path, sep=separator, index=False, encoding="utf-8",
                            quoting=csv.QUOTE_NONNUMERIC)
    else:
        base_vocab[['token']].to_csv(base_output_file_path, sep=separator, index=False, encoding="utf-8",
                                        quoting=csv.QUOTE_NONNUMERIC)

    print(">>> Base vocabulary stored in the file {}".format(base_output_file_path))

    base_vocabulary_file_path = base_output_file_path

    base_vocab_json = [{"name": x, "string": [x]} for x in list(base_vocab.token)]
    with open(base_output_json_file_path, 'w') as fp:
        json.dump(base_vocab_json, fp)
        print(">>> Base vocabulary stored in the file {}".format(base_output_json_file_path))

    print('<< Done vocabulary extraction')

    print(">>> Creating vocabulary including n-grams")

    internal_tokenizer = code_stop_words_tokenizer

    vocab_tokens = set(base_vocab.token)
    
    if token_signature_for_missing:

        def tokenize_with_signatures_for_missing(s):
            tokenized = internal_tokenizer(s)
            result = []
            for token in tokenized:
                if token in vocab_tokens:
                    result.append(token)
                else:
                    result.append(token_signature(token))
            return result


        print(">>> Using a tokenizer that will use token signatures for tokens outside of the base vocabulary")
        tokenizer = tokenize_with_signatures_for_missing
    else:

        def tokenize_skipping_missing(s):
            tokenized = internal_tokenizer(s)
            result = []
            for token in tokenized:
                if token in vocab_tokens:
                    result.append(token)
            return result


        print(">>> Using a tokenizer that will skip tokens outside of the base vocabulary")
        tokenizer = tokenize_skipping_missing

    start = time.process_time()

    count_vect = CountVectorizer(ngram_range=(min_ngrams, max_ngrams),
                                 tokenizer=tokenizer, lowercase=False)
    lines_data = pd.read_csv(input_file_path, sep=separator, encoding="utf-8")
    lines_data.contents = lines_data.contents.fillna("")
    contents = list(lines_data.contents)
    count_vect.fit(contents)

    tokens = sorted(count_vect.vocabulary_.keys(), key=count_vect.vocabulary_.get)
    result_vocab = pd.DataFrame({"token": tokens})

    vocab_file_path = os.path.join(strEmbeddingsFolder, 'vocab.csv')

    result_vocab.to_csv(vocab_file_path, sep=separator, index=False, encoding="utf-8", quoting=csv.QUOTE_NONNUMERIC)

    end = time.process_time()
    print(end - start)

    print(">>> Vocabulary saved to {}".format(vocab_file_path))

    print(f'<< Manual features extraction')
    #extractors_to_use = ["PatternSubstringExctractor", "PatternWordExtractor", "BlankLineFeatureExtraction", "CommentStringExtractor", "NoWordsExtractor", "NoCharsExtractor", "RegexpCountingFeatureExtraction"]
    
    extractors_to_use = ["PatternSubstringExctractor", "PatternWordExtractor", "BlankLineFeatureExtraction", "CommentStringExtractor", "RegexpCountingFeatureExtraction"]
    
    add_decision_class = False
    add_contents = True
    manual_features_config = json.load(open(os.path.join(strEmbeddingsFolder, 'manual_features.json')))

    manual_string_counting_features = manual_features_config.get('manual_string_counting_features', [])
    
    manual_whole_word_counting_features = manual_features_config.get('manual_whole_word_counting_features', [])
    
    regexp_counting_features = manual_features_config.get('regexp_counting_features', [])

    output_file_path = os.path.join(strEmbeddingsFolder, 'manual_features.csv')

    extractors = []
    if "PatternSubstringExctractor" in extractors_to_use:
        substring_counting_extractor = SubstringCountingFeatureExtraction(manual_string_counting_features)
        extractors.append(substring_counting_extractor)
        print(">>> Using {}".format("PatternSubstringExctractor"))

    if "PatternWordExtractor" in extractors_to_use:
        manual_whole_word_counting_exractor = WholeWordCountingFeatureExtraction(manual_whole_word_counting_features)
        extractors.append(manual_whole_word_counting_exractor)
        print(">>> Using {}".format("PatternWordExtractor"))
    
    if "PatternWordTokenizedExtractor" in extractors_to_use:
        manual_tokenized_whole_word_counting_exractor = TokenizedWholeWordCountingFeatureExtraction(manual_whole_word_counting_features)
        extractors.append(manual_tokenized_whole_word_counting_exractor)
        print(">>> Using {}".format("PatternWordTokenizedExtractor"))

    if "RegexpCountingFeatureExtraction" in extractors_to_use:
        regexp_counting_exractor = RegexpCountingFeatureExtraction(regexp_counting_features)
        extractors.append(regexp_counting_exractor)
        print(">>> Using {}".format("RegexpCountingFeatureExtraction"))
        
    if "CommentStringExtractor" in extractors_to_use:
        comment_extractor = CommentFeatureExtraction()
        extractors.append(comment_extractor)
        print(">>> Using {}".format("CommentStringExtractor"))

    if "WholeLineCommentFeatureExtraction" in extractors_to_use:
        comment_extractor = WholeLineCommentFeatureExtraction()
        extractors.append(comment_extractor)
        print(">>> Using {}".format("WholeLineCommentFeatureExtraction"))
		
    if "PythonWholeLineCommentFeatureExtraction" in extractors_to_use:
        comment_extractor = PythonWholeLineCommentFeatureExtraction()
        extractors.append(comment_extractor)
        print(">>> Using {}".format("PythonWholeLineCommentFeatureExtraction"))
		
    if "NoWordsExtractor" in extractors_to_use:
        no_words_extractor = WordCountFeatureExtraction()
        extractors.append(no_words_extractor)
        print(">>> Using {}".format("NoWordsExtractor"))

    if "NoCharsExtractor" in extractors_to_use:
        no_chars_extractor = CharCountFeatureExtraction()
        extractors.append(no_chars_extractor)
        print(">>> Using {}".format("NoCharsExtractor"))
        
    if "BlankLineFeatureExtraction" in extractors_to_use:
        blank_line_extractor = BlankLineFeatureExtraction()
        extractors.append(blank_line_extractor)
        print(">>> Using {}".format("BlankLineFeatureExtraction"))
		
    print(">>> Extracting features")

    controller = LineFeaturesExtractionController(extractors,
                                                  input_file_path, output_file_path,
                                                  sep=separator,
                                                  add_decision_class=add_decision_class,
                                                  add_contents=add_contents,
                                                  verbosity=0)
    controller.extract()

    print(">>> Features stored in the file {}".format(output_file_path))

    print(f'<< Done manual features extraction')

    print('<< BOW extraction')
    
    chunk_size = 100000
    max_line_length = 1000

    print(">>> Loading vocabularies")

    output_file_path = os.path.join(strEmbeddingsFolder, 'bow.csv')

    base_vocab = pd.read_csv(base_vocabulary_file_path, sep=separator, encoding="utf-8")
    vocabulary = pd.read_csv(vocab_file_path, sep=separator, encoding="utf-8")

    internal_tokenizer = code_stop_words_tokenizer
    vocab_tokens = set(result_vocab.token)

    if token_signature_for_missing:

        def tokenize_with_signatures_for_missing(s):
            tokenized = internal_tokenizer(s)
            result = []
            for token in tokenized:
                if token in vocab_tokens:
                    result.append(token)
                else:
                    result.append(token_signature(token))
            return result


        print(">>> Using a tokenizer that will use token signatures for tokens outside of the base vocabulary")
        tokenizer = tokenize_with_signatures_for_missing
    else:

        def tokenize_skipping_missing(s):
            tokenized = internal_tokenizer(s)
            result = []
            for token in tokenized:
                if token in vocab_tokens:
                    result.append(token)
            return result


        print(">>> Using a tokenizer that will skip tokens outside of the base vocabulary")
        tokenizer = tokenize_skipping_missing

    print(">>> Extracting features")

    tokens = list(vocabulary['token'])

    # the line below prevents the error that we cannot find something in the 
    # vocabulary that is not there
    # I added it specifically for this file
    # it was not in the original CCFlex script
    tokens = [x for x in tokens if str(x) != 'nan']

    vocab = dict((key, value) for value, key in enumerate(tokens))

    count_vect = CountVectorizer(ngram_range=(min_ngrams, max_ngrams), tokenizer=tokenizer, vocabulary=vocab, lowercase=False)

    lines_data_chunks = pd.read_csv(input_file_path, sep=separator, encoding="utf-8", chunksize=chunk_size)

    for lines_data in lines_data_chunks:
        lines_data.contents = lines_data.contents.fillna("")
        contents = [line if len(line) < max_line_length else line[:max_line_length] for line in lines_data.contents]
        count_vect.fit_transform(contents)
        break

    extractors = [CountVectorizerBasedFeatureExtraction(count_vect, separator, "{}".format(separator))]

    controller = LineFeaturesExtractionController(extractors,
                                                  input_file_path, output_file_path,
                                                  sep=separator,
                                                  add_decision_class=add_decision_class,
                                                  add_contents=add_contents)
    controller.extract()

    print(">>> Bag of words features saved to file {}".format(output_file_path))
    
    print('<< Done BOW extraction')

    print(f'<< Merge inputs')

    print(">>> Starting to merge the input csv files...")

    output_file_path = os.path.join(strEmbeddingsFolder, 'embeddings1.csv')
    output_file = 'embeddings1.csv'

    input_files = [os.path.join(strEmbeddingsFolder, 'manual_features.csv'), 
                   os.path.join(strEmbeddingsFolder, 'bow.csv')]

    input_file_path = input_files[0]
    first_input_file_df_chunks = pd.read_csv(input_file_path, sep=separator,
                                             encoding="utf-8", chunksize=chunk_size, iterator=False)

    input_files_dfs_chunks = []
    for i, input_file in enumerate(input_files):
        if i > 0:
            input_file_path = input_file
            input_df_chunks = pd.read_csv(input_file_path, sep=separator,
                                          encoding="utf-8", chunksize=chunk_size, iterator=False)
            input_files_dfs_chunks.append(input_df_chunks)

    with open(output_file_path, 'w', newline='', encoding="utf-8") as f:

        for i, first_input_file_df_chunk in enumerate(first_input_file_df_chunks):
            contents_column = None
            decision_class_name_column = None
            decision_class_value_column = None
            merged_df = None

            columns = list(first_input_file_df_chunk.columns)
            if decision_class_name_column is None and 'class_value' in columns:
                decision_class_name_column = first_input_file_df_chunk['class_name']
                decision_class_value_column = first_input_file_df_chunk['class_value']

            if contents_column is None and 'contents' in columns:
                contents_column = first_input_file_df_chunk['contents']

            columns_to_drop = []
            if 'class_value' in columns:
                columns_to_drop.append('class_value')
                columns_to_drop.append('class_name')
            if 'contents' in columns:
                columns_to_drop.append('contents')

            first_input_file_df_chunk = first_input_file_df_chunk.drop(columns_to_drop, inplace=False, axis=1)
            chunks_to_merge = [first_input_file_df_chunk]

            for input_file_dfs_chunks in input_files_dfs_chunks:
                try:
                    input_df_chunk = input_file_dfs_chunks.get_chunk()
                except StopIteration as e:
                    pass

                print(">>> Merging {} lines of the file {}".format(input_df_chunk.shape[0], input_file))

                columns = list(input_df_chunk.columns)
                if decision_class_name_column is None and 'class_value' in columns:
                    decision_class_name_column = input_df_chunk['class_name']
                    decision_class_value_column = input_df_chunk['class_value']

                if contents_column is None and 'contents' in columns:
                    contents_column = input_df_chunk['contents']

                columns_to_drop = ['id']
                if 'class_value' in columns:
                    columns_to_drop.append('class_value')
                    columns_to_drop.append('class_name')
                if 'contents' in columns:
                    columns_to_drop.append('contents')
                input_df_chunk.drop(columns_to_drop, inplace=True, axis=1)

                chunks_to_merge.append(input_df_chunk)

            merged_df = pd.concat(chunks_to_merge, axis=1)

            for chunk in chunks_to_merge:
                del chunk
            del chunks_to_merge
            gc.collect()

            if add_decision_class and decision_class_name_column is not None:
                merged_df['class_name'] = decision_class_name_column
                merged_df['class_value'] = decision_class_value_column

            if add_contents and contents_column is not None:
                merged_df['contents'] = contents_column

            print(">>> Saving the merged chunks to file {}".format(output_file))
            merged_df.to_csv(f, sep=separator, index=False, encoding="utf-8", header=(i == 0),
                             quoting=csv.QUOTE_NONNUMERIC)
            del merged_df
            gc.collect()
            
    print('<< Done merging inputs')  

# method to calculate average embeddings
def create_average_embeddings_ccflex(strEmbeddingsFolder):
    dfAllLines = pd.read_csv(os.path.join(strEmbeddingsFolder, 'embeddings1.csv'), sep=';')
    # create a column with the name of the file
    # which is based on the id column
    dfFile = dfAllLines.id

    # this is the place where we split the name of the file and the line
    # and leave only the line
    dfFile = dfFile.apply(lambda x: x.split(':')[0])

    # and only the name of the file
    dfFile = dfFile.apply(lambda x: x.split('/')[-1])

    # here we drop the features that are not needed
    # ! IMPORTANT - if we want to drop the size-related columns, this is the place
    dfAllLines.drop(columns=['id','contents'], axis=1, inplace=True)

    # add the name of the file as a column
    dfAllLines['file'] = dfFile

    # group by will do the trick 
    # my normalization is average per count of the lines
    # so that we do not get problems that larger programs are per definition further away
    dfEmbeddings = dfAllLines.groupby('file', axis=0).mean() #/dfAllLines.groupby('file', axis=0).count()

    # save the embeddings to a file
    dfEmbeddings.to_csv(os.path.join(strEmbeddingsFolder, 'embeddings_ccflex.csv'), sep=';')

# method to calculate the distances between the analyzed code and the reference code
def calculate_distances_codex(strEmbeddingsFolder):
    ''' Takes the embeddings matrix as input per module and calculates the distances
        between the analyzed code and the reference code. The output is a dataframe
        with the distances between the analyzed code and the reference code. '''
    
    strDir = strEmbeddingsFolder
    strFile = os.path.join(strDir, 'embeddings.csv')

    print(f'<< Reading the embeddings from the file: {strFile}')

    dfEmbeddings = pd.read_csv(strFile, index_col=0, sep=';')

    print(f'<< Adding the classes to the embeddings')

    dfEmbeddings['Modified'] = 0

    dfEmbeddings.loc[dfEmbeddings.index.str.contains('VCE_'), 'Modified'] = 2
    dfEmbeddings.loc[dfEmbeddings.index.str.contains('SCE_'), 'Modified'] = 1

    dfEmbeddings.embeddings = dfEmbeddings.embeddings.apply(eval).to_list()

    # distance calculation   
    print(f'<< Calculating the distances')

    dictReferenceEmbeddings = dfEmbeddings[dfEmbeddings.Modified > 0].to_dict(orient='index')
    dictCodeEmbeddings = dfEmbeddings[dfEmbeddings.Modified == 0].to_dict(orient='index')

    dictCodeEmbeddingsL = {}
    dictReferenceEmbeddingsL ={}

    # dict values to list
    for oneEl in dictCodeEmbeddings.keys():
        dictCodeEmbeddingsL[oneEl] = dictCodeEmbeddings[oneEl]['embeddings']

    # and the same for reference embeddings
    for oneEl in dictReferenceEmbeddings.keys():
        dictReferenceEmbeddingsL[oneEl] = dictReferenceEmbeddings[oneEl]['embeddings']
        

    # list of lists with the results
    lstRes = []

    for refKeys in dictReferenceEmbeddingsL.keys():
        for modKeys in dictCodeEmbeddingsL.keys():
            distance = spatial.distance.cosine(dictReferenceEmbeddingsL[refKeys], dictCodeEmbeddingsL[modKeys])
            oneDistance = [refKeys, modKeys, distance]
            lstRes.append(oneDistance)
            #print(f'Distance from {refKeys} to {modKeys}: {distance:.3f}')

    dfDistances = pd.DataFrame.from_records(lstRes, columns=['Reference', 'Module', 'Distance'])
    
    dfDistances.to_csv(os.path.join(strEmbeddingsFolder, 'distances.csv'))

    print(f'<< Done calculating the distances')

    return dfDistances

def calculate_distances_ccflex(strEmbeddingsFolder):
    ''' Takes the embeddings matrix as input per module and calculates the distances
        between the analyzed code and the reference code. The output is a dataframe
        with the distances between the analyzed code and the reference code. '''
    
    strDir = strEmbeddingsFolder
    strFile = os.path.join(strDir, 'embeddings_ccflex.csv')

    print(f'<< Reading the embeddings from the file: {strFile}')

    dfEmbeddings = pd.read_csv(strFile, index_col=0, sep=';')

    print(f'<< Adding the classes to the embeddings')

    dfEmbeddings['Modified'] = 0

    dfEmbeddings.loc[dfEmbeddings.index.str.contains('VCE_'), 'Modified'] = 2
    dfEmbeddings.loc[dfEmbeddings.index.str.contains('SCE_'), 'Modified'] = 1

    # distance calculation   
    print(f'<< Calculating the distances')

    dictReferenceEmbeddings = dfEmbeddings[dfEmbeddings.Modified > 0].to_dict(orient='index')
    dictCodeEmbeddings = dfEmbeddings[dfEmbeddings.Modified == 0].to_dict(orient='index')

    # print(dictCodeEmbeddings)

    dictCodeEmbeddingsL = {}
    dictReferenceEmbeddingsL ={}

    # dict values to list
    for oneEl in dictCodeEmbeddings.keys():
        dictCodeEmbeddingsL[oneEl] = list(dictCodeEmbeddings[oneEl].values())

    # and the same for reference embeddings
    for oneEl in dictReferenceEmbeddings.keys():
        dictReferenceEmbeddingsL[oneEl] = list(dictReferenceEmbeddings[oneEl].values())
        

    # list of lists with the results
    lstRes = []

    for refKeys in dictReferenceEmbeddingsL.keys():
        for modKeys in dictCodeEmbeddingsL.keys():
            distance = spatial.distance.cosine(dictReferenceEmbeddingsL[refKeys], dictCodeEmbeddingsL[modKeys])
            oneDistance = [refKeys, modKeys, distance]
            lstRes.append(oneDistance)
            #print(f'Distance from {refKeys} to {modKeys}: {distance:.3f}')

    dfDistances = pd.DataFrame.from_records(lstRes, columns=['Reference', 'Module', 'Distance'])
    
    dfDistances.to_csv(os.path.join(strEmbeddingsFolder, 'distances_ccflex.csv'))

    print(f'<< Done calculating the distances')

    return dfDistances



# method to check if a module is secure or not
def analyze_codex(strEmbeddingsFolder):
    
    ''' Analyzes the code and prints the results. The input is the list of distances
        between the analyzed code and the reference code. The output is the verdict
        whether the analyzed code is secure or vulnerable. 
        The verdict is based on the top 3 closest reference programs'''

    strDir = strEmbeddingsFolder
    strFile = os.path.join(strDir, 'distances.csv')

    print(f'<< Reading the distances from the file: {strFile}')

    dfDistances = pd.read_csv(strFile, index_col=0)

    # for each module, visualize the distances 
    # between that module and the reference code samples

    # get unique list of modules
    modules = dfDistances.Module.unique()

    # for each module - find the three closest reference programs
    # and check if they are SCEs or VCEs
    for module in modules:
        # get the distances for the current module
        dfModule = dfDistances[dfDistances['Module'] == module]
        
        # sort the distances
        dfModule = dfModule.sort_values(by=['Distance'])
        
        # get the top 3
        dfModule = dfModule.head(1)
        
        # get the names of the top 3
        dfModuleNames = dfModule['Module']
        
        # get the distances of the top 3
        dfModuleDistances = dfModule['Distance']
        
        # get the labels of the top 3
        dfModuleLabels = dfModule['Reference']

        # print(dfModuleLabels)

        # check if they are SCEs or VCEs
        iSCEs = 0
        iVCEs = 0
        for label in dfModuleLabels:
            if label.startswith('SCE'):
                iSCEs += 1
            else:
                iVCEs += 1  
    
        # write the verdict
        strVerdict = 'secure' if iSCEs > iVCEs else 'vulnerable'  

        mName = module.split('/')[-1]

        # print the verdict
        # changed to printing only the violations and then the examples
        if strVerdict == 'vulnerable':
            print('*****')
            print(f'Module {mName} is flagged as {strVerdict}, with the following reference violations:')

            # here we print only the vulnerable modules and not all three
            # in order to not confuse the user
            for label in dfModuleLabels:
                if label.startswith('VCE'):
                    print(f'>>>> {label}')

    return 1

    # method to check if a module is secure or not
def analyze_ccflex(strEmbeddingsFolder):
    
    ''' Analyzes the code and prints the results. The input is the list of distances
        between the analyzed code and the reference code. The output is the verdict
        whether the analyzed code is secure or vulnerable. 
        The verdict is based on the top 3 closest reference programs'''

    strDir = strEmbeddingsFolder
    strFile = os.path.join(strDir, 'distances_ccflex.csv')

    print(f'<< Reading the distances from the file: {strFile}')

    dfDistances = pd.read_csv(strFile, index_col=0)

    # for each module, visualize the distances 
    # between that module and the reference code samples

    # get unique list of modules
    modules = dfDistances.Module.unique()

    # for each module - find the three closest reference programs
    # and check if they are SCEs or VCEs
    for module in modules:
        # get the distances for the current module
        dfModule = dfDistances[dfDistances['Module'] == module]
        
        # sort the distances
        dfModule = dfModule.sort_values(by=['Distance'])
        
        # get the top 3
        dfModule = dfModule.head(1)        
        
        # get the names of the top 3
        dfModuleNames = dfModule['Module']
        
        # get the distances of the top 3
        dfModuleDistances = dfModule['Distance']
        
        # get the labels of the top 3
        dfModuleLabels = dfModule['Reference']

        # print(dfModuleLabels)

        # check if they are SCEs or VCEs
        iSCEs = 0
        iVCEs = 0
        for label in dfModuleLabels:
            
            if label.startswith('SCE'):
                iSCEs += 1
            else:
                iVCEs += 1  
    
        # write the verdict
        strVerdict = 'secure' if iSCEs > iVCEs else 'vulnerable'  

        mName = module.split('/')[-1]

        # print the verdict
        # changed to printing only the violations and then the examples
        if strVerdict == 'vulnerable':
            print('*****')
            print(f'Module {mName} is flagged as {strVerdict}, with the following reference violations:')

            # here we print only the vulnerable modules and not all three
            # in order to not confuse the user
            for label in dfModuleLabels:
                if label.startswith('VCE'):
                    print(f'>>>> {label}')
            

    return 1