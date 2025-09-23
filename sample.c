
#include "include/raylib.h"
#include <stddef.h>

int main(void)
{
    const int screenWidth = 500;
    const int screenHeight = 500;

    InitWindow(screenWidth, screenHeight, "Test");
 
    char* message = "Small message";
    char* message2 = "A much longer message that might not fit in the box";
    char* message3 = "Very very very long message that definitely will not fit in the box and should be truncated";
   
    int index_start = 0;
    int index_end = strlen(message);
    int messageNum = 1;
    char* displayedMessage = NULL;
    copyPartString(&displayedMessage,message,index_start,index_end);
  
    bool exitProgram = false;

    while (!WindowShouldClose()&&!exitProgram)
    {
        
        BeginDrawing();
        ClearBackground(DARKBLUE);

        DrawRectangle(100, 100, 300, 100, LIGHTGRAY);
        DrawText(displayedMessage, 110, 110, 20, BLACK);

        if(IsKeyReleased(KEY_SPACE)){
           if(index_end>=strlen(message)) {
               index_start=0;
               index_end=0;
              
               if(messageNum==1){
                   message=message2;
               }else if(messageNum==2){
                   message=message3;
               }else{
                   exitProgram=true;
               }
                messageNum++;
            }

            index_start=index_end;
            index_end+=1;

            bool foundMaxLength = false;

            while(!foundMaxLength){
                char* newString = NULL;

                copyPartString(&newString,message,index_start,index_end);

                int textWidth = MeasureText(newString, 20);
                if(textWidth>280){
                    foundMaxLength=true;
                    index_end--;
                    free(newString);
                }else{
                    index_end++;
                    free(displayedMessage);
                    displayedMessage = newString;

                    if(index_end>=strlen(message)){
                        foundMaxLength=true;
                    }
                }
            }

        }

        EndDrawing();
    }

 
    CloseWindow();

    return 0;
}

void copyPartString(char** dest, const char* src, int start, int end){
    *dest = (char*)MemAlloc(end-start+1);
    for(int i=0;i<end-start;i++){
        (*dest)[i]=src[start+i];
    }
    (*dest)[end-start]='\0';
}
