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


