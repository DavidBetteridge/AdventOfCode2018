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

buildChildNodes :: Int -> [Char] -> Int -> (Int, [Node])
buildChildNodes 0 text currentOffset = (currentOffset, [])
buildChildNodes numberOfChildren text currentOffset = (finalOffset, newNode : otherNodes)
            where (newOffset, newNode) = buildNode text currentOffset
                  (finalOffset, otherNodes) = buildChildNodes (numberOfChildren - 1) text newOffset

readMetaData :: Int -> [Char] -> Int -> (Int, [Int])
readMetaData 0 text currentOffset = (currentOffset, [])
readMetaData numberOfChildren text currentOffset = (finalOffset, newMetaData : otherMetaData)
            where newMetaDataAsText = readInt $ skipText text currentOffset
                  newOffset = currentOffset + 1 + length newMetaDataAsText
                  newMetaData = read newMetaDataAsText :: Int
                  (finalOffset, otherMetaData) = readMetaData (numberOfChildren - 1) text newOffset

buildNode :: [Char] -> Int -> (Int, Node)
buildNode contents offset = 
                        (newOffset, 
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
                        (startOfMetaDataEntries, theChildNodes) = buildChildNodes (read theNumberOfChildNodes :: Int) contents startOfChildNodes
                        (newOffset, theMetaData) = readMetaData (read theAmountOfMetaData :: Int) contents (startOfMetaDataEntries)

sumMetaDataNodes :: [Node] -> Int                        
sumMetaDataNodes [] = 0
sumMetaDataNodes (x:xs) = sumMetaData x + sumMetaDataNodes xs

sumMetaData :: Node -> Int
sumMetaData node = sum(metaData node) + sumMetaDataNodes (childNodes node)

test = print (sumMetaData n)
    where (newOffset, n) = buildNode "2 3 0 3 10 11 12 1 1 0 1 99 2 1 1 2" 0
