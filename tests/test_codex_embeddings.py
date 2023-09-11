import pytest
import pandas as pd
import os
import sys

import codex_embeddings

# define some constants for testing
TEST_FOLDER = './test_data'
TEST_CODE_FOLDER = os.path.join(TEST_FOLDER, 'code')
TEST_REFERENCE_FOLDER = os.path.join(TEST_FOLDER, 'reference')
TEST_EMBEDDINGS_FILE = os.path.join(TEST_FOLDER, 'embeddings_codex.csv')
TEST_CODEX_KEY_FILE = 'tests/openAI_key.txt'

# define some fixtures for testing
@pytest.fixture
def embeddings_df():
  # create a dummy embeddings dataframe
  data = {
    'code1.c': [0.4, 0.2, 0.3, 0.4],
    'code2.c': [0.9, 0.6, 0.7, 0.6],
    'SCE_1.c': [0.9, 0.8, 0.7, 0.6],
    'VCE_1.c': [0.4, 0.5, 0.6, 0.7]
  }
  df = pd.DataFrame.from_dict(data, orient='index')
  return df

# test the get_embedding function
def test_get_embedding():
  theKey = ''

  with open('tests/openAI_key.txt', 'r') as fKey:
        theKey = fKey.readline()

  # check if the function returns a list
  emb = codex_embeddings.get_embedding('int main() {}', theKey=theKey)
  assert isinstance(emb, list)

  # check if the list has the expected length
  assert len(emb) == 2048 # 2048 features