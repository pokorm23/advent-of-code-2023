#!/bin/bash
current_year=$(date +%Y)
year=${2:-$current_year}
num=$(printf "%02d" "$1")

dotnet new aoc -q $num -y $year
sed -i "s/https:\/\/adventofcode.com\/$year\/day\/$num/https:\/\/adventofcode.com\/$year\/day\/$1/g" "src/Pokorm.AdventOfCode/Y$year/Days/Day$num.cs"

PUZZLE_URL="https://adventofcode.com/$year/day/$1/input"
PUZZLE_FILE="src/Pokorm.AdventOfCode/Y$year/Inputs/$num.txt"

curl "${PUZZLE_URL}" -H "cookie: session=${AOC_SESSION_COOKIE}" -o "${PUZZLE_FILE}"

git add .
git commit -m "$year.$1 init"
