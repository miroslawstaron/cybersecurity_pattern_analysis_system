import pytest
import sys

import parse_arguments

# define some constants for testing
VALID_ARGS = ['-vce', './reference', '-me', './code', '-m', 'cylbert']
INVALID_ARGS = ['-vce', './reference', '-me', './code', '-m', 'foo']
HELP_ARGS = ['-h']
EXTRA_ARGS = ['-vce', './reference', '-me', './code', '-m', 'cylbert', '-x', 'bar']

# test the parse_arguments function
def test_parse_arguments():
  # check if the function returns the expected folders and model when given valid arguments
  sys.argv = ['main.py'] + VALID_ARGS
  vceFolder, meFolder, strModel = parse_arguments.parse_arguments()
  assert vceFolder == './reference'
  assert meFolder == './code'
  assert strModel == 'cylbert'

  # check if the function exits with code 1 when given invalid arguments
  sys.argv = ['main.py'] + INVALID_ARGS
  with pytest.raises(SystemExit) as e:
    parse_arguments.parse_arguments()
  assert e.type == SystemExit
  assert e.value.code == 1

  # check if the function exits with code 0 when given help arguments
  sys.argv = ['main.py'] + HELP_ARGS
  with pytest.raises(SystemExit) as e:
    parse_arguments.parse_arguments()
  assert e.type == SystemExit
  assert e.value.code == 0

  # check if the function exits with code 1 when given extra arguments
  sys.argv = ['main.py'] + EXTRA_ARGS
  with pytest.raises(SystemExit) as e:
    parse_arguments.parse_arguments()
  assert e.type == SystemExit
  assert e.value.code == 1