import pytest
from start import r12, rev_str

def test_r12():
  assert r12(5) == 5
  assert r12(-3) == -3
  assert r12(0) == 0

def test_rev_str():
  assert rev_str("hello") == "olleh"
  assert rev_str("racecar") == "racecar"
  assert rev_str("") == ""
  assert rev_str("a") == "a"