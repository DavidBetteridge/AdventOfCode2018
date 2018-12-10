-- ghci
-- : l day8.hs
--main  (should give 42472)
--print (numberOfChildNodes n)
--2 3 0 3 10 11 12 1 1 0 1 99 2 1 1 2
--let n = buildNode "2 3 0 3 10 11 12 1 1 0 1 99 2 1 1 2" 0

import System.IO  
import Text.Read
import Data.Char

main = do  
    handle <- openFile "Input.txt" ReadMode  
    hSetEncoding handle utf8_bom
    contents <- hGetContents handle  
    let (_, n) = buildNode (contents ++ " ")
    let answer = sumMetaData n
    putStr $ show answer  
    putStr "\n"

    let answer2 = part2 n
    putStr $ show answer2  
    putStr "\n"

    hClose handle  

data Node = Node { numberOfChildNodes :: Int, 
                   amountOfMetaData ::Int, 
                   childNodes :: [Node], 
                   metaData :: [Int] }

readInt :: [Char] -> (Int, [Char])
readInt text = (theInt, remainingText)
    where theIntAsText = takeWhile isDigit text
          amountToSkip = 1 + length theIntAsText
          remainingText = skipText text amountToSkip
          theInt = read theIntAsText :: Int

skipText :: [Char] -> Int -> [Char]
skipText text 0 = text
skipText (x:xs) offset = skipText xs (offset - 1)

buildChildNodes :: Int -> [Char] -> ([Node], [Char])
buildChildNodes 0 text = ([], text)
buildChildNodes numberOfChildren text = (newNode : otherNodes, finalText)
            where (text1, newNode) = buildNode text
                  (otherNodes, finalText) = buildChildNodes (numberOfChildren - 1) text1

readMetaData :: Int -> [Char] -> ([Int], [Char])
readMetaData 0 text = ([], text)
readMetaData numberOfChildren text = (newMetaData : otherMetaData, finalText)
            where (newMetaData, text2) = readInt text
                  (otherMetaData, finalText) = readMetaData (numberOfChildren - 1) text2 

buildNode :: [Char] -> ([Char], Node)
buildNode contents =   (contents4, 
                        Node { numberOfChildNodes = theNumberOfChildNodes, 
                               amountOfMetaData = theAmountOfMetaData, 
                               childNodes = theChildNodes, 
                               metaData = theMetaData }
                        )
                    where 
                        (theNumberOfChildNodes, contents1) = readInt contents
                        (theAmountOfMetaData, contents2) = readInt contents1
                        (theChildNodes, contents3) = buildChildNodes theNumberOfChildNodes contents2
                        (theMetaData, contents4) = readMetaData theAmountOfMetaData contents3

sumMetaDataNodes :: [Node] -> Int                        
sumMetaDataNodes [] = 0
sumMetaDataNodes (x:xs) = sumMetaData x + sumMetaDataNodes xs

sumMetaData :: Node -> Int
sumMetaData node = sum(metaData node) + sumMetaDataNodes (childNodes node)

part2 :: Node -> Int
part2 node = if (numberOfChildNodes node == 0) then
                sum(metaData node)
             else
                result
        where 
            suitableNodes = filter (\childNumber -> childNumber > 0 && childNumber <= numberOfChildNodes node) (metaData node)      
            nodesToSum = map (\childNumber -> (childNodes node)!!(childNumber-1)) suitableNodes     
            nodeTotals = map (part2) nodesToSum   
            result = sum(nodeTotals)    

test = print ( (show $ amountOfMetaData n )  ++ " " ++ show m1 ++ " " ++ show m2 ++ " " ++ show m3)
    where (newText, n) = buildNode "2 3 0 3 10 11 12 1 1 0 1 99 2 1 1 2"
          m1:m2:m3 = metaData n

-- Node A  2 3 0 3  (Children Nodes B,C)  (Meta Data 1 1 2)      
-- Node B  0 3 (No Children) (Meta Data 10 11 12)
-- Node C  1 1 (Child 4) (Meta Data 2)
-- Node D  0 1 99 (No Children) Meta Data 99