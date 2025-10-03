'use client';

import { Button, ButtonGroup } from 'react-bootstrap';
import { 
  FaLock, 
  FaUnlock, 
  FaTrash, 
  FaUserSlash 
} from 'react-icons/fa';

interface ToolbarProps {
  selectedCount: number;
  onBlock: () => void;
  onUnblock: () => void;
  onDelete: () => void;
  onDeleteUnverified: () => void;
  disabled?: boolean;
}

export default function Toolbar({
  selectedCount,
  onBlock,
  onUnblock,
  onDelete,
  onDeleteUnverified,
  disabled = false
}: ToolbarProps) {
  
  // Disabling buttons when no users selected
  const isDisabled = disabled || selectedCount === 0;

  return (
    <div className="d-flex gap-2 mb-3 p-3 bg-light border rounded">
      <ButtonGroup>
        <Button
          variant="warning"
          onClick={onBlock}
          disabled={isDisabled}
          title="Block selected users"
        >
          <FaLock className="me-2" />
          Block
        </Button>
        <Button
          variant="success"
          onClick={onUnblock}
          disabled={isDisabled}
          title="Unblock selected users"
        >
          <FaUnlock />
        </Button>
        <Button
          variant="danger"
          onClick={onDelete}
          disabled={isDisabled}
          title="Delete selected users"
        >
          <FaTrash />
        </Button>
        <Button
          variant="secondary"
          onClick={onDeleteUnverified}
          disabled={disabled}
          title="Delete all unverified users"
        >
          <FaUserSlash />
        </Button>
      </ButtonGroup>

      {selectedCount > 0 && (
        <span className="align-self-center ms-3 text-muted">
          {selectedCount} user(s) selected
        </span>
      )}
    </div>
  );
}