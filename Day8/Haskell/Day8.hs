-- ghci
-- : l day8.hs
--main
--print (numberOfChildNodes n)
--2 3 0 3 10 11 12 1 1 0 1 99 2 1 1 2
--let n = buildNode "2 3 0 3 10 11 12 1 1 0 1 99 2 1 1 2" 0

import System.IO  
import Text.Read
import Data.Char

main = do  
    handle <- openFile "Sample.txt" ReadMode  
    hSetEncoding handle utf8_bom
    contents <- hGetContents handle  
    putStr contents  
    hClose handle  


data Node = Node { numberOfChildNodes :: Int, 
                   amountOfMetaData ::Int, 
                   childNodes :: [Node], 
                   metaData :: [Int] }

readInt :: [Char] -> [Char]
readInt = takeWhile isDigit

skipText :: [Char] -> Int -> [Char]
skipText text 0 = text
skipText (x:xs) offset = skipText xs (offset - 1)

buildChildNodes :: Int -> [Char] -> Int -> [Node] -> (Int, [Node])
buildChildNodes 0 text currentOffset builtNodes = (currentOffset, builtNodes)
buildChildNodes numberOfChild text currentOffset builtNodes = (newOffset, builtNodes ++ [newNode])
            where (newOffset, newNode) = buildNode text currentOffset
                

readMetaData :: Int -> [Char] -> Int -> [Int] -> (Int, [Int])
readMetaData 0 text currentOffset builtMetaData = (currentOffset, builtMetaData)
readMetaData numberOfChild text currentOffset builtMetaData = (newOffset, builtMetaData ++ [newMetaData])
            where newMetaDataAsText = readInt $ skipText text currentOffset
                  newOffset = currentOffset + 1 + length newMetaDataAsText
                  newMetaData = read newMetaDataAsText :: Int

buildNode :: [Char] -> Int -> (Int, Node)
buildNode contents offset = 
                        (newOffset , 
                        Node { numberOfChildNodes = read theNumberOfChildNodes :: Int, 
                               amountOfMetaData = read theAmountOfMetaData :: Int, 
                               childNodes = theChildNodes, 
                               metaData = theMetaData }
                        )
                    where 
                        theNumberOfChildNodes = readInt $ skipText contents offset
                        startOfMetaData = offset + 1 + length theNumberOfChildNodes
                        theAmountOfMetaData = readInt $ skipText contents startOfMetaData
                        startOfChildNodes = startOfMetaData + 1 + length theAmountOfMetaData
                        (startOfMetaDataEntries, theChildNodes) = buildChildNodes (read theNumberOfChildNodes :: Int) contents startOfChildNodes []
                        (newOffset, theMetaData) = readMetaData (read theAmountOfMetaData :: Int) contents startOfMetaDataEntries []

test = print (newOffset)
    where (newOffset, n) = buildNode "2 3 0 3 10 11 12 1 1 0 1 99 2 1 1 2" 0
