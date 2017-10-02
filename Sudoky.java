/**
 * Created by max12332 on 30.11.2016.
 */
package Cydoky;
public class Sudoky {
    public static void main(String[] args)
    {
        int[][] mas = { {8,0,0,0,0,0,0,0,0},
                        {0,0,3,6,0,0,0,0,0},
                        {0,7,0,0,9,0,2,0,0},
                        {0,5,0,0,0,7,0,0,0},
                        {0,0,0,0,4,5,7,0,0},
                        {0,0,0,1,0,0,0,3,0},
                        {0,0,1,0,0,0,0,6,8},
                        {0,0,8,5,0,0,0,1,0},
                        {0,9,0,0,0,0,4,0,0}};
        boolean states = false;
        while (true)
        {
            states =false;
            mas = func2(3, 3, func1(mas, 3, 3), mas);
            mas = func2(3, 6, func1(mas, 3, 6), mas);
            mas = func2(3, 9, func1(mas, 3, 9), mas);
            mas = func2(6, 3, func1(mas, 6, 3), mas);
            mas = func2(6, 6, func1(mas, 6, 6), mas);
            mas = func2(6, 9, func1(mas, 6, 9), mas);
            mas = func2(9, 3, func1(mas, 9, 3), mas);
            mas = func2(9, 6, func1(mas, 9, 6), mas);
            mas = func2(9, 9, func1(mas, 9, 9), mas);

            for(int i=0; i<9;i++)
                for (int j=0;j<9;j++)
                    if(mas[i][j] == 0)
                        states = true;
            if(states == false)
                break;
        }
        for(int i =0; i<9;i++){
            for(int j=0;j<9;j++)
                System.out.print(mas[i][j]+ " ");
            System.out.println();
            }
    }

    public static int[] func1(int[][]arr, int n, int m)
    {
        int[] mas= new int[9];
        int[] ans = new int[9];
        int l=0,l1=0;
        for (int k=1 ;k<10; k++)
            for(int i=n-3; i<n; i++){
            for(int j=m-3; j<m;j++)
            {
                    if (arr[i][j] == k) {
                        mas[l] = k;
                        l++;
                        l1 = 1;
                        break;
                    }
            }
        if(l1==1)
        {l1=0;
            break;}

        }

        l=0;
        boolean b = false;
        for(int j=1;j<10;j++){
        for (int i=0; i<9; i++)
            if (mas[i]==j)
            {
               b = true;
                break;
            }
            if (b == false) {
                ans[l]=j;
                l++;
            }
            b=false;
        }
            return ans;
    }

    public static int[][] func2(int n, int m, int[] ans, int[][] mas)
    {
        int r4 =0, count =0;
        int x =0, y =0;
        boolean r1 = false;
        for(int i =n-3;i <n;i++)
            for(int j=m-3;j<m;j++)
                if(mas[i][j]==0) {
                    for(int r =0; r<9;r++)
                    {
                        if(ans[r] != 0) {
                            for (int k = 0; k < 9; k++)
                                if (mas[i][k] == ans[r]) {
                                    r1 = true;
                                    break;
                                }
                            if (r1 == false)
                                for (int k = 0; k < 9; k++)
                                    if (mas[k][j] == ans[r]) {
                                        r1 = true;
                                        break;
                                    }
                            if (r1 == false) {
                                x = i;
                                y = j;
                                count++;
                                r4 = ans[r];
                            }
                            if (count == 2)
                                break;
                            r1 = false;
                        }
                    }
                    if(count ==1)
                        mas[x][y] = r4;
                    r1 = false;
                    count = 0;
                }
        return mas;
    }
}
