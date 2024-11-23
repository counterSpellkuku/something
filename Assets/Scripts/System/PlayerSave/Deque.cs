namespace System.PlayerSave
{
    
public class Deque<T> {
    // 어쩌다보니 arrayList로...
    
    
    private T[] array;
    private int front;
    private int rear;
    private int size;
    private const int DefaultCapacity = 16;

    public Deque()
    {
        array = new T[DefaultCapacity];
        front = 0;
        rear = 0;
        size = 0;
    }

    public int Count => size;

    public bool IsEmpty => size == 0;

    private void EnsureCapacity() {
        if (size == array.Length)
        {
            T[] newArray = new T[array.Length * 2];
            if (front <= rear)
            {
                Array.Copy(array, front, newArray, 0, size);
            }
            else
            {
                Array.Copy(array, front, newArray, 0, array.Length - front);
                Array.Copy(array, 0, newArray, array.Length - front, rear);
            }
            front = 0;
            rear = size;
            array = newArray;
        }
    }

    public void AddFront(T item)
    {
        EnsureCapacity();
        front = (front - 1 + array.Length) % array.Length;
        array[front] = item;
        size++;
    }

    public void AddRear(T item)
    {
        EnsureCapacity();
        array[rear] = item;
        rear = (rear + 1) % array.Length;
        size++;
    }

    public T RemoveFront()
    {
        if (IsEmpty)
            throw new InvalidOperationException("empty");

        T item = array[front];
        array[front] = default(T);
        front = (front + 1) % array.Length;
        size--;
        return item;
    }

    public T RemoveRear()
    {
        if (IsEmpty)
            throw new InvalidOperationException("empty");

        rear = (rear - 1 + array.Length) % array.Length;
        T item = array[rear];
        array[rear] = default(T);
        size--;
        return item;
    }

    public T PeekFront()
    {
        if (IsEmpty)
            throw new InvalidOperationException("empty");

        return array[front];
    }

    public T PeekRear()
    {
        if (IsEmpty)
            throw new InvalidOperationException("empty");

        return array[(rear - 1 + array.Length) % array.Length];
    }

    public void Clear()
    {
        Array.Clear(array, 0, array.Length);
        front = 0;
        rear = 0;
        size = 0;
    }
}
}