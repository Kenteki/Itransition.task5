'use client';

import { useState } from 'react';
import { useRouter } from 'next/navigation';
import { Container, Row, Col, Form, Button, Alert } from 'react-bootstrap';
import { authService } from '@/services/api';
import Link from 'next/link';
import { toast } from 'react-toastify';

export default function RegisterPage() {
  const [name, setName] = useState('');
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [position, setPosition] = useState('');
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);
  const router = useRouter();

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError('');
    setLoading(true);

    try {
      const response = await authService.register({ name, email, password, position });
      toast.success(response.data.message || 'Registration successful! Please check your email.');
      setTimeout(() => {
        router.push('/login');
      }, 4000);
    } catch (error: unknown) {
  if (error instanceof Error) {
    toast.error(error.message);
  } else if (typeof error === 'object' && error !== null && 'response' in error) {
    // Если это AxiosError
    const axiosError = error as { response?: { data?: { message?: string } } };
    toast.error(axiosError.response?.data?.message || 'Registration failed. Please try again.');
  } else {
    toast.error('Registration failed. Please try again.');
  }
} finally {
      setLoading(false);
    }
  };

  return (
    <Container fluid className="vh-100">
      <Row className="h-100">
        <Col md={6} className="d-flex align-items-center justify-content-center bg-white">
          <div style={{ maxWidth: '400px', width: '100%', padding: '2rem' }}>
            <h1 className="fw-bold mb-1" style={{ color: '#0066ff', fontSize: '2rem' }}>
              ItransitionTask5
            </h1>
            
            <div className="mt-5 mb-4">
              <h3 className="fw-bold mb-4">Create Your Account</h3>
            </div>

            {error && <Alert variant="danger">{error}</Alert>}

            <Form onSubmit={handleSubmit}>
              <Form.Group className="mb-3">
                <Form.Label className="text-muted" style={{ fontSize: '0.85rem' }}>
                  Name
                </Form.Label>
                <Form.Control
                  type="text"
                  placeholder="Enter your name"
                  value={name}
                  onChange={(e) => setName(e.target.value)}
                  required
                  style={{ borderRadius: '0.25rem', padding: '0.75rem' }}
                />
              </Form.Group>

              <Form.Group className="mb-3">
                <Form.Label className="text-muted" style={{ fontSize: '0.85rem' }}>
                  E-mail
                </Form.Label>
                <Form.Control
                  type="email"
                  placeholder="test@example.com"
                  value={email}
                  onChange={(e) => setEmail(e.target.value)}
                  required
                  style={{ borderRadius: '0.25rem', padding: '0.75rem' }}
                />
              </Form.Group>

              <Form.Group className="mb-3">
                <Form.Label className="text-muted" style={{ fontSize: '0.85rem' }}>
                  Position
                </Form.Label>
                <Form.Control
                  type="text"
                  placeholder="e.g. Developer, Manager"
                  value={position}
                  onChange={(e) => setPosition(e.target.value)}
                  required
                  style={{ borderRadius: '0.25rem', padding: '0.75rem' }}
                />
              </Form.Group>

              <Form.Group className="mb-3">
                <Form.Label className="text-muted" style={{ fontSize: '0.85rem' }}>
                  Password
                </Form.Label>
                <Form.Control
                  type="password"
                  placeholder="••••••••"
                  value={password}
                  onChange={(e) => setPassword(e.target.value)}
                  required
                  minLength={1}
                  style={{ borderRadius: '0.25rem', padding: '0.75rem' }}
                />
              </Form.Group>

              <Button 
                type="submit" 
                className="w-100 fw-semibold"
                disabled={loading}
                style={{
                  backgroundColor: '#0066ff',
                  border: 'none',
                  borderRadius: '0.25rem',
                  padding: '0.75rem'
                }}
              >
                {loading ? 'Creating account...' : 'Sign Up'}
              </Button>
            </Form>

            <div className="text-center mt-4">
              <span className="text-muted" style={{ fontSize: '0.9rem' }}>
                Already have an account?{' '}
              </span>
              <Link 
                href="/login" 
                style={{ color: '#0066ff', textDecoration: 'none', fontSize: '0.9rem' }}
              >
                Sign in
              </Link>
            </div>
          </div>
        </Col>
            <Col
               md={6}
               className="d-none d-md-block"
               style={{
                  backgroundImage: "url('images/login-img.jpg')",
                  backgroundSize: 'cover',
                  backgroundPosition: 'center',
                  backgroundRepeat: 'no-repeat'
               }}
            ></Col>
         </Row>
      </Container>
   );
}