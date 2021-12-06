def rep(f,n,x):
  if n == 0: return []
  return [x] + rep(f,n-1,f(x))

def rotate90(t): return list(zip(*t[::-1]))

def format(pat):
  return '\n'.join('BYTE ' + ' '.join(str(n) for n in line) for line in pat)

# pattern = [[0,1,0,1,0],[0,0,0,0,0],[0,0,1,0,0],[0,0,0,0,0],[1,1,1,1,1]]
# [print(format(pat),end='\n\n') for pat in rep(rotate90,4,pattern)]

