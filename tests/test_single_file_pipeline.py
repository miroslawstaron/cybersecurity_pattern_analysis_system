import pytest
import pandas as pd
import os
import sys

import single_file_pipeline

# define some constants for testing
TEST_FOLDER = './test_data'
TEST_CODE_FOLDER = os.path.join(TEST_FOLDER, 'code')
TEST_REFERENCE_FOLDER = os.path.join(TEST_FOLDER, 'reference')
TEST_RESULT_FILE = os.path.join(TEST_FOLDER, 'result.csv')
TEST_WORKDIR = os.path.join(TEST_FOLDER, 'workdir')
TEST_MODELS = ['singberta', 'cylbert', 'codebert']

# test the pipeline function
@pytest.mark.parametrize('model', TEST_MODELS)
def test_pipeline(model):
  # check if the function runs without errors
  single_file_pipeline.pipeline(TEST_REFERENCE_FOLDER, TEST_CODE_FOLDER, TEST_RESULT_FILE, TEST_WORKDIR, model)

  # check if the result file is created and not empty
  assert os.path.isfile(TEST_RESULT_FILE)
  assert os.path.getsize(TEST_RESULT_FILE) > 0

  # check if the result file has the expected columns
  df = pd.read_csv(TEST_RESULT_FILE)
  assert 'file' in df.columns
  assert 'distance' in df.columns
  assert 'vulnerability' in df.columns
  assert 'pattern' in df.columns