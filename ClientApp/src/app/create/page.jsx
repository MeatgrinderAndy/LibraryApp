'use client'

import { useState, useEffect } from 'react'
import { useRouter } from 'next/navigation'
import axios from 'axios'
import { jwtDecode } from 'jwt-decode'

export default function AddBookPage() {
  const [form, setForm] = useState({
    title: '',
    isbn: '',
    genre: '',
    description: '',
    authorId: ''
  })
  const [image, setImage] = useState(null)
  const [isAdmin, setIsAdmin] = useState(false)
  const [authors, setAuthors] = useState([])
  const router = useRouter()

  useEffect(() => {
    const fetchAuthors = async () => {
      try {
        const res = await axios.get('https://localhost:7001/api/authors')
        setAuthors(res.data)
      } catch (err) {
        console.error('Ошибка при загрузке авторов', err)
      }
    }

    const token = localStorage.getItem('token')
    if (token) {
      const decoded = jwtDecode(token)
      const role = decoded['http://schemas.microsoft.com/ws/2008/06/identity/claims/role']
      setIsAdmin(role === 'Admin')
      fetchAuthors() 
    }
  }, [])

  const handleSubmit = async () => {
    try {
      const formData = new FormData();
      const bookData = {
        title: form.title,
        isbn: form.isbn,
        genre: form.genre,
        description: form.description,
        authorId: Number(form.authorId)
      };
  
      formData.append('bookDto', JSON.stringify(bookData));
      
      if (image) {
        formData.append('coverImage', image);
      }
  
      await axios.post('https://localhost:7001/api/books', formData, {
        headers: {
          'Authorization': `Bearer ${localStorage.getItem('token')}`,
          'Content-Type': 'multipart/form-data'
        }
      });
  
      router.push('/books');
    } catch (err) {
      console.error('Ошибка при добавлении книги', err);
      alert('Произошла ошибка при добавлении книги');
    }
  }

  if (!isAdmin) {
    return <p style={{ textAlign: 'center', marginTop: 20 }}>Доступ запрещен</p>
  }

  return (
    <div style={{ maxWidth: 600, margin: 'auto', padding: 20 }}>
      <h2>Добавить новую книгу</h2>
      
      <div style={{ marginBottom: 20, padding: 10, border: '1px solid #ddd', borderRadius: 5 }}>
        <h3>Доступные авторы:</h3>
        {authors.length > 0 ? (
          <ul style={{ listStyle: 'none', padding: 0 }}>
            {authors.map(author => (
              <li key={author.id} style={{ marginBottom: 5 }}>
                <strong>ID: {author.id}</strong> - {author.firstName} {author.lastName}
              </li>
            ))}
          </ul>
        ) : (
          <p>Загрузка списка авторов...</p>
        )}
      </div>

      <div style={{ display: 'flex', flexDirection: 'column', gap: 10 }}>
        <input
          type="text"
          value={form.title}
          onChange={(e) => setForm({ ...form, title: e.target.value })}
          placeholder="Название"
        />
        <input
          type="text"
          value={form.isbn}
          onChange={(e) => setForm({ ...form, isbn: e.target.value })}
          placeholder="ISBN"
        />
        <input
          type="text"
          value={form.genre}
          onChange={(e) => setForm({ ...form, genre: e.target.value })}
          placeholder="Жанр"
        />
        <input
          type="text"
          value={form.authorId}
          onChange={(e) => setForm({ ...form, authorId: e.target.value })}
          placeholder="ID автора"
        />
        <textarea
          value={form.description}
          onChange={(e) => setForm({ ...form, description: e.target.value })}
          placeholder="Описание"
          rows={4}
        />
        <input 
          type="file" 
          onChange={(e) => setImage(e.target.files[0])} 
        />
        <div style={{ marginTop: 20 }}>
          <button onClick={handleSubmit}>Добавить книгу</button>
          <button 
            onClick={() => router.push('/')} 
            style={{ marginLeft: 10 }}
          >
            Отмена
          </button>
        </div>
      </div>
    </div>
  )
}