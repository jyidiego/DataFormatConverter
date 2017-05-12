#!/bin/bash

pushd ./PoCIWM/DataFormatConverter/test
    dotnet restore
    dotnet test
    ret_code=$?
popd
exit $ret_code
