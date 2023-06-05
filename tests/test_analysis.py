'''
This is the test file for analysis.py
'''

import analysis
import pytest
import json

def test_extract_embeddings_ccflex():
  # test the function that extracts the embeddings matrix from the analyzed code
  # using a mock input and output

  # mock the input parameters
  strEmbeddingsFolder = 'test_folder'
  strCodeFolder = 'test_code'

  # mock the expected output
  expected_output = 1 # the function returns 1 at the end

  # call the function
  actual_output = analysis.extract_embeddings_ccflex(strEmbeddingsFolder, strCodeFolder)

  # assert that the output matches the expected output
  assert actual_output == expected_output

  # assert that the output files are created in the test_folder
  assert os.path.exists(os.path.join(strEmbeddingsFolder, 'lines.csv'))
  assert os.path.exists(os.path.join(strEmbeddingsFolder, 'vocab.csv'))
  assert os.path.exists(os.path.join(strEmbeddingsFolder, 'manual_features.csv'))
  assert os.path.exists(os.path.join(strEmbeddingsFolder, 'bow.csv'))
  assert os.path.exists(os.path.join(strEmbeddingsFolder, 'embeddings1.csv'))

def test_extract_embeddings_ccflex_error():
  # test the function that extracts the embeddings matrix from the analyzed code
  # using an invalid input that should raise an error

  # mock the input parameters
  strEmbeddingsFolder = 'test_folder'
  strCodeFolder = 'test_code'

  # mock the invalid input
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

  # mock the json.loads function to return the invalid input
  json.loads = lambda x: strLocations

  # assert that the function raises an Error
  with pytest.raises(Error):
    analysis.extract_embeddings_ccflex(strEmbeddingsFolder, strCodeFolder)
