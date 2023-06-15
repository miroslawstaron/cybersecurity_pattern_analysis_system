import pytest
from start import r12, rev_str
from start import check_leap_year
from start import quickSort

def test_r12():
  assert r12(5) == 5
  assert r12(-3) == -3
  assert r12(0) == 0

def test_rev_str():
  assert rev_str("hello") == "olleh"
  assert rev_str("racecar") == "racecar"
  assert rev_str("") == ""
  assert rev_str("a") == "a"
def test_check_leap_year():
  assert check_leap_year(2000) == True
  assert check_leap_year(1900) == False
  assert check_leap_year(2004) == False
  assert check_leap_year(2003) == False
  assert check_leap_year(1600) == True
  assert check_leap_year(1700) == False
  assert check_leap_year(0) == True 
  assert check_leap_year(-4) == False
  with pytest.raises(NameError):
    check_leap_year(year) # undefined variable

def test_quickSort():
  assert quickSort([3, 1, 4, 5, 2]) == [1, 2, 3, 4, 5]
  assert quickSort([10, 9, 8, 7, 6]) == [6, 7, 8, 9, 10]
  assert quickSort([1, 2, 3, 4, 5]) == [1, 2, 3, 4, 5]
  assert quickSort([]) == []
  assert quickSort([1]) == [1]
  assert quickSort([5, 3, 7, 2, 9, 4, 6, 8, 1]) == [1, 2, 3, 4, 5, 6, 7, 8, 9]
  assert quickSort([3, 3, 3, 3, 3]) == [3, 3, 3, 3, 3]
  with pytest.raises(TypeError):
    quickSort(None) # invalid input
  with pytest.raises(TypeError):
    quickSort([1, 2, "a"]) # mixed types