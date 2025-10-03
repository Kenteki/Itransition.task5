'use client';

import { useEffect, useState } from 'react';
import { Container } from 'react-bootstrap';
import { useRouter } from 'next/navigation';
import { useAuth } from '@/contexts/AuthContext';
import { userService, User } from '@/services/api';
import { toast } from 'react-toastify';
import NavigationBar from '@/components/Navbar';
import Toolbar from '@/components/Toolbar';
import UserTable from '@/components/UserTable';

export default function AdminPage() {
  const [users, setUsers] = useState<User[]>([]);
  const [selectedUsers, setSelectedUsers] = useState<Set<string>>(new Set());
  const [loading, setLoading] = useState(true);
  const { isAuthenticated, user } = useAuth();
  const router = useRouter();

  // Check authentication status
  useEffect(() => {
    if (!isAuthenticated) {
      router.push('/login');
    }
  }, [isAuthenticated, router]);

  useEffect(() => {
    if (isAuthenticated) {
      loadUsers();
    }
  }, [isAuthenticated]);

  // Fetch all users from API
const loadUsers = async () => {
    try {
      setLoading(true);
      const response = await userService.getAllUsers();
      setUsers(response.data);
    } catch (error: unknown) {
  if (error instanceof Error) {
    toast.error(error.message);
  } else if (typeof error === 'object' && error !== null && 'response' in error) {
    // Если это AxiosError
    const axiosError = error as { response?: { data?: { message?: string } } };
    toast.error(axiosError.response?.data?.message || 'Failed to load users');
  } else {
    toast.error('Failed to load users');
  }
} finally {
      setLoading(false);
    }
  };

  // Select all checkbox handler
  const handleSelectAll = (checked: boolean) => {
    if (checked) {
      setSelectedUsers(new Set(users.map(u => u.id)));
    } else {
      setSelectedUsers(new Set());
    }
  };

  // Individual user selection handler
  const handleSelectUser = (userId: string, checked: boolean) => {
    const newSelected = new Set(selectedUsers);
    if (checked) {
      newSelected.add(userId);
    } else {
      newSelected.delete(userId);
    }
    setSelectedUsers(newSelected);
  };

  // Block selected users
  const handleBlock = async () => {
    if (selectedUsers.size === 0) return;

    try {
      await userService.blockUsers(Array.from(selectedUsers));
      toast.success(`Successfully blocked ${selectedUsers.size} user(s)`);
      if (selectedUsers.has(user?.userId)) {
        toast.info('You blocked yourself. Logging out...');
        setTimeout(() => {
          router.push('/login');
        }, 2000);
      } else {
        setSelectedUsers(new Set());
        loadUsers();
      }
    } catch (error: unknown) {
  if (error instanceof Error) {
    toast.error(error.message);
  } else if (typeof error === 'object' && error !== null && 'response' in error) {
    // Если это AxiosError
    const axiosError = error as { response?: { data?: { message?: string } } };
    toast.error(axiosError.response?.data?.message || 'Failed to block users');
  } else {
    toast.error('Failed to block action');
  }
}

  };

  // Unblock selected users
  const handleUnblock = async () => {
    if (selectedUsers.size === 0) return;

    try {
      await userService.unblockUsers(Array.from(selectedUsers));
      toast.success(`Successfully unblocked ${selectedUsers.size} user(s)`);
      setSelectedUsers(new Set());
      loadUsers();
    } catch (error: unknown) {
  if (error instanceof Error) {
    toast.error(error.message);
  } else if (typeof error === 'object' && error !== null && 'response' in error) {
    // Если это AxiosError
    const axiosError = error as { response?: { data?: { message?: string } } };
    toast.error(axiosError.response?.data?.message || 'Failed to unblock users');
  } else {
    toast.error('Failed to unblock action');
  }
}
  };

  // Delete selected users
  const handleDelete = async () => {
    if (selectedUsers.size === 0) return;
    if (!confirm(`Are you sure you want to delete ${selectedUsers.size} user(s)? This action cannot be undone.`)) {
      return;
    }

    try {
      await userService.deleteUsers(Array.from(selectedUsers));
      toast.success(`Successfully deleted ${selectedUsers.size} user(s)`);
      
      // Check if current user deleted themselves
      if (selectedUsers.has(user?.userId)) {
        toast.info('You deleted yourself. Logging out...');
        setTimeout(() => {
          router.push('/login');
        }, 2000);
      } else {
        setSelectedUsers(new Set());
        loadUsers();
      }
    } catch (error: unknown) {
  if (error instanceof Error) {
    toast.error(error.message);
  } else if (typeof error === 'object' && error !== null && 'response' in error) {
    // Если это AxiosError
    const axiosError = error as { response?: { data?: { message?: string } } };
    toast.error(axiosError.response?.data?.message || 'Failed to delete users');
  } else {
    toast.error('Failed to delete action');
  }
}
  };

  // Delete all unverified users
  const handleDeleteUnverified = async () => {
    if (!confirm('Are you sure you want to delete all unverified users? This action cannot be undone.')) {
      return;
    }

    try {
      const response = await userService.deleteUnverified();
      toast.success(response.data.message || 'Successfully deleted unverified users');
      setSelectedUsers(new Set());
      loadUsers();
    } catch (error: unknown) {
  if (error instanceof Error) {
    toast.error(error.message);
  } else if (typeof error === 'object' && error !== null && 'response' in error) {
    // Если это AxiosError
    const axiosError = error as { response?: { data?: { message?: string } } };
    toast.error(axiosError.response?.data?.message || 'Failed to delete unverified users');
  } else {
    toast.error('Failed to delete unverified action');
  }
}
  };

  if (!isAuthenticated) {
    return null;
  }

  return (
    <>
      <NavigationBar />
      <Container fluid className="px-4">
        <h2 className="mb-4">User Management</h2>
        <Toolbar
          selectedCount={selectedUsers.size}
          onBlock={handleBlock}
          onUnblock={handleUnblock}
          onDelete={handleDelete}
          onDeleteUnverified={handleDeleteUnverified}
          disabled={loading}
        />

        {loading ? (
          <div className="text-center py-5">
            <div className="spinner-border" role="status">
              <span className="visually-hidden">Loading...</span>
            </div>
          </div>
        ) : (
          <UserTable
            users={users}
            selectedUsers={selectedUsers}
            onSelectAll={handleSelectAll}
            onSelectUser={handleSelectUser}
          />
        )}
      </Container>
    </>
  );
}