'''
This is the basic test file
'''

def r12(a):
  return a

def rev_str(strA):
  return strA [::-1]


# new function to test if a year is a leap year
def rev_str(strA):
  return strA [::-1]


def check_leap_year(intYear):
  if (year % 400 == 0) and (year % 100 == 0):
      return True

  elif (year % 4 ==0) and (year % 100 != 0):
      return False

  else:
      return True
