import pytest
import pandas as pd
import os
import sys

import cylberta_embeddings

# define some constants for testing
TEST_FOLDER = './test_data'
TEST_CODE_FOLDER = os.path.join(TEST_FOLDER, 'code')
TEST_REFERENCE_FOLDER = os.path.join(TEST_FOLDER, 'reference')
TEST_EMBEDDINGS_FILE = os.path.join(TEST_FOLDER, 'embeddings_cylberta.csv')
TEST_DISTANCES_FILE = os.path.join(TEST_FOLDER, 'distances_cylberta.csv')

# define some fixtures for testing
@pytest.fixture
def embeddings_df():
  # create a dummy embeddings dataframe
  data = {
    'code1.c': [0.4, 0.2, 0.3, 0.4, 0],
    'code2.c': [0.9, 0.6, 0.7, 0.6, 0],
    'SCE_1.c': [0.9, 0.8, 0.7, 0.6, 1],
    'VCE_1.c': [0.4, 0.5, 0.6, 0.7, 2]
  }
  df = pd.DataFrame.from_dict(data, orient='index', columns=['e1', 'e2', 'e3', 'e4', 'Modified'])
  return df

@pytest.fixture
def distances_df():
  # create a dummy distances dataframe
  data = {
    'Reference': ['SCE_1.c', 'SCE_1.c', 'VCE_1.c', 'VCE_1.c'],
    'Module': ['code1.c', 'code2.c', 'code1.c', 'code2.c'],
    'Distance': [0.1, 0.2, 0.3, 0.4]
  }
  df = pd.DataFrame.from_dict(data)
  return df

# test the extract_embeddings_cylberta function
def test_extract_embeddings_cylberta():
  # check if the function returns a dataframe
  df = cylberta_embeddings.extract_embeddings_cylberta(TEST_FOLDER, TEST_CODE_FOLDER, TEST_REFERENCE_FOLDER)
  assert isinstance(df, pd.DataFrame)
 
  # check if the dataframe has the expected shape and columns
  assert df.shape == (5, 768) # 5 files, 768 features
  
  # check if the dataframe is saved to a file
  assert os.path.exists(TEST_EMBEDDINGS_FILE)

# test the calculate_distances_cylberta function
def test_calculate_distances_cylberta(embeddings_df):
  # mock the embeddings file with the fixture
  embeddings_df.to_csv(TEST_EMBEDDINGS_FILE, sep='$')

  # check if the function returns a dataframe
  df = cylberta_embeddings.calculate_distances_cylberta(TEST_FOLDER)
  assert isinstance(df, pd.DataFrame)
  
  # check if the dataframe has the expected shape and columns
  assert df.shape == (4, 3) # 4 distances, 3 columns
  assert list(df.columns) == ['Reference', 'Module', 'Distance']
  
  # check if the dataframe has the expected values for the Distance column
  # using some arbitrary thresholds
  assert df.loc[(df['Reference'] == 'SCE_1.c') & (df['Module'] == 'code1.c'), 'Distance'].values[0] > 0.2
  assert df.loc[(df['Reference'] == 'SCE_1.c') & (df['Module'] == 'code2.c'), 'Distance'].values[0] < 0.6

  assert df.loc[(df['Reference'] == 'VCE_1.c') & (df['Module'] == 'code1.c'), 'Distance'].values[0] < 0.6
  assert df.loc[(df['Reference'] == 'VCE_1.c') & (df['Module'] == 'code2.c'), 'Distance'].values[0] > 0.2
  
  # check if the dataframe is saved to a file
  assert os.path.exists(TEST_DISTANCES_FILE)

# test the analyze_cylberta function
def test_analyze_cylberta(distances_df, capsys):
  # mock the distances file with the fixture
  distances_df.to_csv(TEST_DISTANCES_FILE, sep=',')

  # check if the function returns 1
  assert cylberta_embeddings.analyze_cylberta(TEST_FOLDER) == 1
  
  # check if the function prints the expected output
  captured = capsys.readouterr()
  assert 'Module code1.c is flagged as vulnerable' in captured.out
  assert '>>>> VCE_1.c' in captured.out
  assert 'Module code2.c is flagged as secure' not in captured.out
  assert '>>>> SCE_1.c' not in captured.out