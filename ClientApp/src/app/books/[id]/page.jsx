'use client'

import { useEffect, useState } from 'react'
import { useParams, useRouter } from 'next/navigation'
import axios from 'axios'
import { jwtDecode } from 'jwt-decode'

export default function BookDetailsPage() {
  const [book, setBook] = useState(null)
  const [isAdmin, setIsAdmin] = useState(false)
  const [editMode, setEditMode] = useState(false)
  const [form, setForm] = useState({})
  const [image, setImage] = useState(null)

  const router = useRouter()
  const params = useParams()
  const { id } = params

  useEffect(() => {
    const token = localStorage.getItem('token')
    if (token) {
      const decoded = jwtDecode(token)
      const role = decoded['http://schemas.microsoft.com/ws/2008/06/identity/claims/role']
      setIsAdmin(role === 'Admin')
    }

    fetchBook()
  }, [])

  const fetchBook = async () => {
    try {
      const res = await axios.get(`http://localhost:7001/api/books/${id}`)
      setBook(res.data)
      setForm({
        title: res.data.title,
        isbn: res.data.isbn,
        genre: res.data.genre,
        description: res.data.description,
        authorId: res.data.authorId,
        authorName: res.data.authorName
      })
    } catch (err) {
      console.error('Ошибка при получении книги', err)
    }
  }

  const handleDelete = async () => {
    try {
      await axios.delete(`http://localhost:7001/api/books/${id}`, {
        headers: {
          Authorization: `Bearer ${localStorage.getItem('token')}`
        }
      })
      router.push('/books')
    } catch (err) {
      console.error('Ошибка при удалении книги', err)
    }
  }
  const handleUpdate = async () => {
    try {
      const formData = new FormData();
      formData.append('Title', form.title);
      formData.append('Isbn', form.isbn);
      formData.append('Genre', form.genre);
      formData.append('Description', form.description);
      formData.append('AuthorId', form.authorId);
        
      if (image) {
        formData.append('coverImage', image);
      }
  
      for (let [key, value] of formData.entries()) {
        console.log(key, value);
      }
  
      const response = await axios.put(`http://localhost:7001/api/books/${id}`, formData, {
        headers: {
          'Authorization': `Bearer ${localStorage.getItem('token')}`,
          'Content-Type': 'multipart/form-data'
        }
      });
  
      setBook(prev => ({ ...prev, ...response.data }));
      setEditMode(false);
    } catch (err) {
      console.error('Full error:', err);
      console.error('Error response:', err.response);
      console.error('Error data:', err.response?.data);
    }
  };

  if (!book) return <p>Загрузка...</p>

  return (
    <div style={{ maxWidth: 600, margin: 'auto' }}>
      {editMode ? (
        <>
          <h2>Редактирование книги</h2>
          <input
            type="text"
            value={form.title || ''}
            onChange={(e) => setForm({ ...form, title: e.target.value })}
            placeholder="Название"
          />
          <input
            type="text"
            value={form.isbn || ''}
            onChange={(e) => setForm({ ...form, isbn: e.target.value })}
            placeholder="ISBN"
          />
          <input
            type="text"
            value={form.genre || ''}
            onChange={(e) => setForm({ ...form, genre: e.target.value })}
            placeholder="Жанр"
          />
          <textarea
            value={form.description || ''}
            onChange={(e) => setForm({ ...form, description: e.target.value })}
            placeholder="Описание"
          />
          <input type="file" onChange={(e) => setImage(e.target.files[0])} />
          <br />
          <button onClick={handleUpdate}>Сохранить</button>
          <button onClick={() => setEditMode(false)}>Отмена</button>
        </>
      ) : (
        <>
          <h2>{book.title}</h2>
          {book.imageUrl && <img src={book.imageUrl} alt={book.title} width={200} />}
          {book.coverImage && (
        <img
          src={`data:image/jpeg;base64,${book.coverImage}`}
          alt="Обложка книги"
          style={{ width: '150px', height: 'auto', marginBottom: 10 }}
        />
      )}
          <p><strong>ISBN:</strong> {book.isbn}</p>
          <p><strong>Автор:</strong> {book.authorName}</p>
          <p><strong>Жанр:</strong> {book.genre}</p>
          <p><strong>Описание:</strong> {book.description}</p>
          <p><strong>Дата взятия:</strong> {book.dateWhenTaken || '—'}</p>
          <p><strong>Вернуть до:</strong> {book.dateWhenNeedToReturn || '—'}</p>

          {isAdmin && (
            <div style={{ marginTop: 20 }}>
              <button onClick={() => setEditMode(true)}>Редактировать</button>
              <button onClick={handleDelete} style={{ marginLeft: 10 }}>Удалить</button>
            </div>
          )}
        </>
      )}
    </div>
  )
}