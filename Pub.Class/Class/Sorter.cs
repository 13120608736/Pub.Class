using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;

namespace Pub.Class {
    /// <summary>
    /// HeapSorter ������ O(n*log2n)~O(n*log2n) | O(1) 
    /// 
    /// �޸ļ�¼
    ///     2010.01.12 �汾��1.0 livexy ��������
    /// 
    /// </summary>    
    public static class HeapSorter<T> where T : System.IComparable {
        #region Sort
        /// <summary>
        /// ������
        /// </summary>
        /// <param name="list"></param>
        /// <param name="isAsc"></param>
        public static void Sort(IList<T> list, bool isAsc) {
            //��������򣬽��ѵ�����һ�����ѣ�����ǽ��򣬵�������С��
            for (int i = list.Count / 2 - 1; i >= 0; i--) {
                HeapSorter<T>.Adjust(list, i, list.Count - 1, isAsc);
            }
            //�Ѹ��������һ������ֵ������Ȼ��ѳ����������Ľ������ɶ�
            for (int i = list.Count - 1; i > 0; i--) {
                list.Swap<T>(0, i);
                HeapSorter<T>.Adjust(list, 0, i - 1, isAsc);
            }
        }
        #endregion

        #region Adjust
        /// <summary>
        /// ������
        /// </summary>
        /// <param name="list"></param>
        /// <param name="nodeIndx"></param>
        /// <param name="maxAdjustIndx"></param>
        /// <param name="isAsc"></param>
        private static void Adjust(IList<T> list, int nodeIndx, int maxAdjustIndx, bool isAsc) {
            T rootValue = list[nodeIndx];
            T temp = list[nodeIndx];
            int childNodeIndx = 2 * nodeIndx + 1;
            while (childNodeIndx <= maxAdjustIndx) {
                if (isAsc) {
                    if (childNodeIndx < maxAdjustIndx && list[childNodeIndx].CompareTo(list[childNodeIndx + 1]) < 0) {
                        childNodeIndx++;
                    }
                    if (rootValue.CompareTo(list[childNodeIndx]) > 0) {
                        break;
                    } else {
                        list[(childNodeIndx - 1) / 2] = list[childNodeIndx];
                        childNodeIndx = 2 * childNodeIndx + 1;
                    }
                } else {
                    if (childNodeIndx < maxAdjustIndx && list[childNodeIndx].CompareTo(list[childNodeIndx + 1]) > 0) {
                        childNodeIndx++;
                    }
                    if (rootValue.CompareTo(list[childNodeIndx]) < 0) {
                        break;
                    } else {
                        list[(childNodeIndx - 1) / 2] = list[childNodeIndx];
                        childNodeIndx = 2 * childNodeIndx + 1;
                    }
                }

            }
            list[(childNodeIndx - 1) / 2] = temp;
        }
        #endregion
    }
    /// <summary>
    /// InsertionSorter �������� O(n2) | O(1) 
    /// 
    /// �޸ļ�¼
    ///     2010.01.12 �汾��1.0 livexy ��������
    /// 
    /// </summary>
    public static class InsertionSorter<T> where T : System.IComparable {
        #region Sort
        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="list"></param>
        /// <param name="isAsc"></param>
        public static void Sort(IList<T> list, bool isAsc) {
            T next;
            int j;
            for (int i = 1; i < list.Count; i++) {
                next = list[i];
                if (isAsc) {
                    for (j = i - 1; j >= 0 && list[j].CompareTo(next) > 0; j--) {
                        list[j + 1] = list[j];
                    }
                } else {
                    for (j = i - 1; j >= 0 && list[j].CompareTo(next) < 0; j--) {
                        list[j + 1] = list[j];
                    }
                }
                list[j + 1] = next;
            }
        }
        #endregion
    }
    /// <summary>
    /// MergeSorter �鲢����
    /// 
    /// �޸ļ�¼
    ///     2010.01.12 �汾��1.0 livexy ��������
    /// 
    /// </summary>    
    public static class MergeSorter<T> where T : System.IComparable {
        #region Sort
        /// <summary>
        /// �鲢����
        /// </summary>
        /// <param name="list"></param>
        /// <param name="isAsc"></param>
        public static void Sort(IList<T> list, bool isAsc) {
            int length = 1;
            IList<T> sortedList = new List<T>(list.Count);
            foreach (T item in list) {
                sortedList.Add(item);
            }
            while (length < list.Count) {
                MergeSorter<T>.MergePass(list, sortedList, length, isAsc);
                length *= 2;
                MergeSorter<T>.MergePass(sortedList, list, length, isAsc);
                length *= 2;
            }
        }
        #endregion

        #region private
        private static void MergePass(IList<T> list, IList<T> sortedList, int length, bool isASC) {
            int i, j;
            for (i = 0; i <= list.Count - 2 * length; i += 2 * length) {
                MergeSorter<T>.Merge(list, i, i + length - 1, i + 2 * length - 1, sortedList, isASC);
            }
            if (i + length < list.Count) {
                MergeSorter<T>.Merge(list, i, i + length - 1, list.Count - 1, sortedList, isASC);
            } else {
                for (j = i; j < list.Count; j++) {
                    sortedList[j] = list[j];
                }
            }

        }
        /// <summary>
        /// �鲢�����Ѿ������������Ϊһ�����������
        /// </summary>
        /// <param name="list">ԭʼ����</param>
        /// <param name="startIndx">�鲢��ʼindex</param>
        /// <param name="splitIndx">��һ�ν�����index</param>
        /// <param name="endIndx">�ڶ��ν�����index</param>
        /// <param name="sortedList">����������</param>
        /// <param name="isASC">�Ƿ�����</param>
        private static void Merge(IList<T> list, int startIndx, int splitIndx, int endIndx, IList<T> sortedList, bool isASC) {
            int right = splitIndx + 1;
            int left = startIndx;
            int sortIndx = startIndx;
            while (left <= splitIndx && right <= endIndx) {
                if (isASC) {
                    if (list[left].CompareTo(list[right]) <= 0) {
                        sortedList[sortIndx++] = list[left++];
                    } else {
                        sortedList[sortIndx++] = list[right++];
                    }
                } else {
                    if (list[left].CompareTo(list[right]) >= 0) {
                        sortedList[sortIndx++] = list[left++];
                    } else {
                        sortedList[sortIndx++] = list[right++];
                    }
                }
            }
            if (left > splitIndx) {
                for (int t = right; t <= endIndx; t++) {
                    sortedList[sortIndx + t - right] = list[t];
                }
            } else {
                for (int t = left; t <= splitIndx; t++) {
                    sortedList[sortIndx + t - left] = list[t];
                }
            }
        }

        #endregion
    }
    /// <summary>
    /// QuickSorter �������� O(n*log2n)~O(n2) | O(log2n)~O(n) 
    /// 
    /// �޸ļ�¼
    ///     2010.01.12 �汾��1.0 livexy ��������
    /// 
    /// </summary>
    public static class QuickSorter<T> where T : System.IComparable {
        /// <summary>
        /// ��������
        /// </summary>
        /// <param name="list"></param>
        /// <param name="isAsc"></param>
        public static void Sort(IList<T> list, bool isAsc) {
            QuickSorter<T>.Sort(list, 0, list.Count - 1, isAsc);
        }

        #region Sort
        private static void Sort(IList<T> list, int left, int right, bool isAsc) {
            int i, j;
            T pivot;
            if (left < right) {
                i = left;
                j = right + 1;
                if (isAsc) {
                    while (i < j) {
                        i++;
                        pivot = list[left];
                        while (i < right && list[i].CompareTo(pivot) < 0) {
                            i++;
                        }
                        j--;
                        while (j >= left && list[j].CompareTo(pivot) > 0) {
                            j--;
                        }
                        if (i < j) {
                            list.Swap<T>(i, j);
                        }
                    }
                } else {
                    while (i < j) {
                        i++;
                        pivot = list[left];
                        while (i < right && list[i].CompareTo(pivot) > 0) {
                            i++;
                        }
                        j--;
                        while (j >= left && list[j].CompareTo(pivot) < 0) {
                            j--;
                        }
                        if (i < j) {
                            list.Swap<T>(i, j);
                        }
                    }

                }
                list.Swap<T>(left, j);
                QuickSorter<T>.Sort(list, left, j - 1, isAsc);
                QuickSorter<T>.Sort(list, j + 1, right, isAsc);
            }
        }
        #endregion
    }
    //���������� O(n*log2n)~O(n2) | O(n)
    //ϣ������ O | O(1)
    //��������>�鲢����>������>ϣ������
}
