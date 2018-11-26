#!/bin/sh

if [ "$#" -ne 2 ]; then
  echo "Usage: $0 <input_file> <target_price>" >&2
  exit 1
fi

dotnet Challenge2.dll $1 $2 2
