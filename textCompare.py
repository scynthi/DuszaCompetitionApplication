import os

og_test_cases_1 = r"tesztesetek\01";
og_test_cases_2 = r"tesztesetek\02";
my_test_files = r"TestFiles";
compare_files = ""


names = os.listdir(my_test_files)
inp = int(input("Melyik test(1, 2): "))

if (inp == 1):
    compare_files = og_test_cases_1
else:
    compare_files = og_test_cases_2

my_test_files_lines = []
compare_files_lines = []

for name in names:
    with open(my_test_files + "\\" + name, "r", encoding="utf8") as f:
        my_test_files_lines.append(f.readlines())
    with open(compare_files + "\\" + name, "r", encoding="utf8") as f:
        compare_files_lines.append(f.readlines())

similar = True
wrong_my = []
wrong_comp = []

for i in range(len(my_test_files_lines)):
    for j in range(len(my_test_files_lines[i])):
        print(my_test_files_lines[i][j] + "\t" + compare_files_lines[i][j])
        if my_test_files_lines[i][j] != compare_files_lines[i][j]:
            similar = False
            wrong_my.append(my_test_files_lines[i][j])
            wrong_comp.append(compare_files_lines[i][j])


if similar:
    print("The two directories are the same!")
else:
    print("The two aren't the same!")
    for i in range(len(wrong_my)):
        print(wrong_my[i] + "\t" + wrong_comp[i])

input()