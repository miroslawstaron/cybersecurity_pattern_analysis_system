'''
This is the basic test file
'''

def r12(a):
  return a

def rev_str(strA):
  return strA [::-1]

def rev_str(strA):
  return strA [::-1]

# check if a year is a leap year
def check_leap_year(intYear):
  if (intYear % 4 == 0) and (intYear % 100 != 0):
    if (intYear % 400 == 0):
      return True
  elif (intYear % 4 != 0):
      return False
  elif (intYear % 400 == 0):  
    return True
  
  return False

def quickSort(arr):
    less = []
    pivotList = []
    more = []
    if len(arr) <= 1:
        return arr
    else:
        pivot = arr[0]
        for i in arr:
            if i < pivot:
                less.append(i)
            elif i > pivot:
                more.append(i)
            else:
                pivotList.append(i)
        less = quickSort(less)
        more = quickSort(more)
        return less + pivotList + more
