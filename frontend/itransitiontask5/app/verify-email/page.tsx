'use client';
import { useEffect, useState, Suspense } from 'react';
import { useRouter, useSearchParams } from 'next/navigation';
import { Container, Card, Spinner, Alert } from 'react-bootstrap';
import { authService } from '@/services/api';
import Link from 'next/link';

export default function VerifyEmailPage() {
  return (
    <Suspense fallback={<div>Loading...</div>}>
      <VerifyEmailContent />
    </Suspense>
  );
}

function VerifyEmailContent() {
  const [status, setStatus] = useState<'loading' | 'success' | 'error'>('loading');
  const [message, setMessage] = useState('');
  const searchParams = useSearchParams();
  const router = useRouter();

  useEffect(() => {
    const token = searchParams.get('token');
    if (!token) {
      setStatus('error');
      setMessage('Invalid verification link.');
      return;
    }

    authService.verifyEmail(token)
      .then((res) => {
        setStatus('success');
        setMessage(res.data.message || 'Email verified successfully!');
        setTimeout(() => router.push('/login'), 3000);
      })
      .catch((err) => {
        setStatus('error');
        setMessage(err.response?.data?.message || 'Email verification failed.');
      });
  }, [searchParams, router]);

  return (
    <Container className="d-flex align-items-center justify-content-center" style={{ minHeight: '100vh' }}>
      <Card className="shadow-sm" style={{ maxWidth: '500px' }}>
        <Card.Body className="text-center p-5">
          {status === 'loading' && (
            <>
              <Spinner animation="border" variant="primary" className="mb-3" />
              <h4>Verifying your email...</h4>
            </>
          )}
          {status === 'success' && (
            <>
              <div className="text-success mb-3" style={{ fontSize: '3rem' }}>✓</div>
              <Alert variant="success">{message}</Alert>
              <p className="text-muted">Redirecting to login page...</p>
              <Link href="/login" className="btn btn-primary mt-3">Go to Login</Link>
            </>
          )}
          {status === 'error' && (
            <>
              <div className="text-danger mb-3" style={{ fontSize: '3rem' }}>✗</div>
              <Alert variant="danger">{message}</Alert>
              <Link href="/register" className="btn btn-primary mt-3">Register Again</Link>
            </>
          )}
        </Card.Body>
      </Card>
    </Container>
  );
}
