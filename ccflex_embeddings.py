#!/bin/python3
#############################################################
#
# Secure design pattern and vulnerabilities detection system
#
# (c) Miroslaw Staron, 2021-2023
# www.staron.nu
#
#############################################################

import json
import os

from prepare import LinesCaseExtractor, VocabularyExtractor, code_stop_words_tokenizer, token_signature
from prepare import LineFeaturesExtractionController, CountVectorizerBasedFeatureExtraction

from sklearn.feature_extraction.text import CountVectorizer

import pandas as pd

from pathlib import Path 

dummy_decision_classes ={
    "default" :{
        'name' : "_",
        'value' : "0"
    }
}

def _ccflex_create_locations(strRefFolder, strMeFolder, strEmbeddingsFolder, code_file_patterns):
    locations = {
        "vce" : {
            "baseline_dir": "/",
            "locations": [
                {"path": strRefFolder,
                "include": code_file_patterns,
                "exclude": []
                },
            ]
        },
        "me" : {
            "baseline_dir": "/",
            "locations": [
                {"path": strMeFolder,
                "include": code_file_patterns,
                "exclude": []
                },
            ]
        },
        "workspace_dir": {
            "path": strEmbeddingsFolder,
            "erase": False
        }
    }
    locations["all"] = locations["vce"]
    locations["all"]["locations"] += locations["me"]["locations"]
    return locations

def _ccflex_extract_lines(all_lines_path, locations):
    
    lines_extractor = LinesCaseExtractor(code_location = locations, 
                                        output_file_path = all_lines_path, 
                                        decision_classes = dummy_decision_classes,
                                        sep='$',
                                        quotechar="\"",
                                        remove_duplicates=True)
    lines_extractor.extract()


def _ccflex_get_tokenizer(token_signature_for_missing, base_vocab):
    internal_tokenizer = code_stop_words_tokenizer
    vocab_tokens = set(base_vocab)
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


        print("<< Using a tokenizer that will use token signatures for tokens outside of the base vocabulary")
        tokenizer = tokenize_with_signatures_for_missing
    else:

        def tokenize_skipping_missing(s):
            tokenized = internal_tokenizer(s)
            result = []
            for token in tokenized:
                if token in vocab_tokens:
                    result.append(token)
            return result


        logger.info(">>> Using a tokenizer that will skip tokens outside of the base vocabulary")
        tokenizer = tokenize_skipping_missing
    return tokenizer


def _ccflex_features_to_embeddings(features_file_path):
    features_df = pd.read_csv(features_file_path, sep='$')
    features_df['file'] = features_df['id'].apply(lambda x: Path(x.split(':')[0]).name)
    features_df.drop(columns=['id',], axis=1, inplace=True)
    embeddings_df = features_df.groupby('file', axis=0).mean() #/dfAllLines.groupby('file', axis=0).count()

    for column in embeddings_df.columns:
        if column != "file":
            embeddings_df[column] = (embeddings_df[column] - embeddings_df[column].min()) / (embeddings_df[column].max() - embeddings_df[column].min())    

    return embeddings_df



# method to extract embeddings matrix
# @strEmbeddingsFolder - the path to the folder where the embeddings matrix will be stored
# @strRefFolder - the path to the folder with the reference code
# @strMeFolder - the path to the folder with the code to be analyzed
# @vocab_manual - the manual vocabulary
# @vocab_top_words_threshold - limit the size of vocabulary to that number of tokens
# @bow_vocab_size - the size of the bag of words vocabulary; if 0, then the BOW is not used
# @vocab_bow_ref_only - whether to use only reference code to build vocab and bow (if False it uses both reference and code to be analyzed)
# @remove_temp_files - whether or not it shall remove all the temporary files produced while generating embeddings
# @code_file_patterns - patterns to code files - by default all files
def extract_embeddings_ccflex(strEmbeddingsFolder, 
                              strRefFolder, 
                              strMeFolder,
                              vocab_manual = ['main', 'string', 'int'],
                              vocab_top_words_threshold = 10000,
                              token_signature_for_missing = True,
                              min_ngrams = 1,
                              max_ngrams = 2,
                              bow_vocab_size = 100,
                              vocab_bow_ref_only=False,
                              remove_temp_files=True,
                              code_file_patterns = [".+",]):
    ''' Extracts the embeddings matrix from the analyzed code. The input is the path
        to the analyzed code. The output is the embeddings matrix. '''
    
    locations = _ccflex_create_locations(strRefFolder, strMeFolder, strEmbeddingsFolder, code_file_patterns)

    # Extract lines of code
    print(f'<< Extracting lines from files')
    all_lines_path = os.path.join(strEmbeddingsFolder, 'all-lines.csv')
    _ccflex_extract_lines(all_lines_path, locations["all"])

    if vocab_bow_ref_only:
        ref_lines_path = os.path.join(strEmbeddingsFolder, 'vce-lines.csv')
        _ccflex_extract_lines(all_lines_path, locations["vce"])

    # Build base vocabulary
    print(f'<< Building base vocabulary')
    if vocab_bow_ref_only:
        vocab_lines_path = ref_lines_path
    else:
        vocab_lines_path = all_lines_path
    
    vocab_extractor = VocabularyExtractor(vocab_lines_path, "$")
    vocab_extractor.extract()
    base_vocab = vocab_extractor.vocab
    if vocab_top_words_threshold > 0:
        print("<< Taking {} most frequent words".format(vocab_top_words_threshold))
        base_vocab = base_vocab.head(vocab_top_words_threshold)
    base_vocab = list(set(vocab_manual + base_vocab['token'].tolist()))
    print(f"<< Extracted base vocabulary including {len(base_vocab)} tokens")

    print(f"<< Extending vocab with n-grams")
    tokenizer = _ccflex_get_tokenizer(token_signature_for_missing, base_vocab)
    count_vect = CountVectorizer(ngram_range=(min_ngrams, max_ngrams),
                                tokenizer=tokenizer, lowercase=False, max_features=bow_vocab_size)
    lines_data = pd.read_csv(vocab_lines_path, sep="$", encoding="utf-8")
    lines_data.contents = lines_data.contents.fillna("")
    contents = list(lines_data.contents)
    count_vect.fit(contents)

    print("<< Extracting features...")
    extractors = [CountVectorizerBasedFeatureExtraction(count_vect, "$", "{}".format("$"))]

    features_file_path = os.path.join(strEmbeddingsFolder, 'features.csv')
    controller = LineFeaturesExtractionController(extractors,
                                                  all_lines_path, features_file_path,
                                                  sep="$",
                                                  add_decision_class=False,
                                                  add_contents=False)
    controller.extract()

    embeddings_df = _ccflex_features_to_embeddings(features_file_path)

    embeddings_df.to_csv(os.path.join(strEmbeddingsFolder, 'embeddings_ccflex.csv'), sep=';')

    if remove_temp_files:
        print("<< Removing temporary files")
        os.remove(all_lines_path)
        if vocab_bow_ref_only:
            os.remove(ref_lines_path)
        os.remove(features_file_path)



# method to extract embeddings matrix
# @strEmbeddingsFolder - the path to the folder where the embeddings matrix will be stored
# @strRefFolder - the path to the folder with the reference code
# @strMeFolder - the path to the folder with the code to be analyzed
# @vocab_manual - the manual vocabulary
# @vocab_top_words_threshold - limit the size of vocabulary to that number of tokens
# @bow
# @bow_vocab_size - the size of the bag of words vocabulary; if 0, then the BOW is not used
def extract_embeddings_ccflex2(strEmbeddingsFolder, 
                              strRefFolder, 
                              strMeFolder,
                              vocab_manual = ['main', 'string', 'int'],
                              vocab_top_words_threshold = 10000,
                              token_signature_for_missing = True,
                              min_ngrams = 1,
                              max_ngrams = 2,
                              bow_vocab_size = 100):
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