import pytest
from start import r12, rev_str
from start import check_leap_year

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
