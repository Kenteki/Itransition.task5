'use client';

import { useState } from 'react';
import { useRouter } from 'next/navigation';
import { Container, Row, Col, Form, Button, Alert } from 'react-bootstrap';
import { authService } from '@/services/api';
import { useAuth } from '@/contexts/AuthContext';
import { toast } from 'react-toastify';
import Link from 'next/link';

export default function LoginPage() {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);
  const router = useRouter();
  const { login } = useAuth();

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError('');
    setLoading(true);

    try {
      const response = await authService.login({ email, password });
      const { token, userId, name, email: userEmail, status } = response.data;
      login(token, { userId, name, email: userEmail, status });
      router.push('/admin');
    } catch (error: unknown) {
      if (error instanceof Error) {
        toast.error(error.message);
      } else if (typeof error === 'object' && error !== null && 'response' in error) {
        // Если это AxiosError
        const axiosError = error as { response?: { data?: { message?: string } } };
        toast.error(axiosError.response?.data?.message || 'Login failed. Please try again.');
      } else {
        toast.error('Login failed. Please try again.');
      }
    } finally {
      setLoading(false);
    }
  };

  return (
    <Container fluid className="vh-100">
      <Row className="h-100">
        {/* Left Side - Form */}
        <Col md={6} className="d-flex align-items-center justify-content-center bg-white">
          <div style={{ maxWidth: '400px', width: '100%', padding: '2rem' }}>
            <h1 className="fw-bold mb-1" style={{ color: '#0066ff', fontSize: '2rem' }}>
              ItransitionTask5
            </h1>
            
            <div className="mt-5 mb-4">
              <h3 className="fw-bold mb-4">Sign In to The App</h3>
            </div>

            {error && <Alert variant="danger">{error}</Alert>}

            <Form onSubmit={handleSubmit}>
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
                  style={{ 
                    borderRadius: '0.25rem',
                    border: '1px solid #dee2e6',
                    padding: '0.75rem'
                  }}
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
                  style={{ 
                    borderRadius: '0.25rem',
                    border: '1px solid #dee2e6',
                    padding: '0.75rem'
                  }}
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
                  padding: '0.75rem',
                  fontSize: '1rem'
                }}
              >
                {loading ? 'Signing in...' : 'Sign In'}
              </Button>
            </Form>

            <div className="d-flex justify-content-between mt-4">
              <div>
                <span className="text-muted" style={{ fontSize: '0.9rem' }}>
                  Don&apos;t have an account?{' '}
                </span>
                <Link 
                  href="/register" 
                  style={{ color: '#0066ff', textDecoration: 'none', fontSize: '0.9rem' }}
                >
                  Sign up
                </Link>
              </div>
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
        >
        </Col>
      </Row>
    </Container>
  );
}