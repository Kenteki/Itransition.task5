'use client';

import { Table, Form } from 'react-bootstrap';
import { User } from '@/services/api';
import { formatDistanceToNow } from 'date-fns';

interface UserTableProps {
  users: User[];
  selectedUsers: Set<string>;
  onSelectAll: (checked: boolean) => void;
  onSelectUser: (userId: string, checked: boolean) => void;
}

export default function UserTable({ 
  users, 
  selectedUsers, 
  onSelectAll, 
  onSelectUser 
}: UserTableProps) {
  
  const allSelected = users.length > 0 && selectedUsers.size === users.length;

  const formatDateTime = (dateString: string | null) => {
    if (!dateString) return 'Never';
    try {
      return formatDistanceToNow(new Date(dateString), { addSuffix: true });
    } catch {
      return 'Invalid date';
    }
  };

  const getStatusBadge = (status: string) => {
    switch (status.toLowerCase()) {
      case 'active':
        return 'success';
      case 'blocked':
        return 'danger';
      case 'unverified':
        return 'warning';
      default:
        return 'secondary';
    }
  };

  return (
    <div className="table-responsive">
      <Table striped bordered hover>
        <thead>
          <tr>
            <th style={{ width: '50px' }}>
              <Form.Check
                type="checkbox"
                checked={allSelected}
                onChange={(e) => onSelectAll(e.target.checked)}
              />
            </th>
            <th>Name</th>
            <th>Email</th>
            <th>Position</th>
            <th>Last Login</th>
            <th>Status</th>
          </tr>
        </thead>
        <tbody>
          {users.length === 0 ? (
            <tr>
              <td colSpan={6} className="text-center">No users found</td>
            </tr>
          ) : (
            users.map((user) => (
              <tr key={user.id}>
                <td>
                  <Form.Check
                    type="checkbox"
                    checked={selectedUsers.has(user.id)}
                    onChange={(e) => onSelectUser(user.id, e.target.checked)}
                  />
                </td>
                <td>{user.name}</td>
                <td>{user.email}</td>
                <td>{user.position || 'N/A'}</td>
                <td>{formatDateTime(user.lastLogin)}</td>
                <td>
                  <span className={`badge bg-${getStatusBadge(user.status)}`}>
                    {user.status}
                  </span>
                </td>
              </tr>
            ))
          )}
        </tbody>
      </Table>
    </div>
  );
}