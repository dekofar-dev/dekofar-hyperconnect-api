import { Component, OnInit } from '@angular/core';
import { RoleService } from './role.service';

@Component({
  selector: 'app-role-management',
  templateUrl: './role-management.component.html'
})
export class RoleManagementComponent implements OnInit {
  roles: string[] = [];
  newRole = '';
  selectedUserId = '';
  userRoles: string[] = [];

  constructor(private roleService: RoleService) {}

  ngOnInit(): void {
    this.loadRoles();
  }

  loadRoles(): void {
    this.roleService.getRoles().subscribe(r => (this.roles = r));
  }

  createRole(): void {
    if (!this.newRole.trim()) {
      return;
    }
    this.roleService.createRole(this.newRole).subscribe(() => {
      this.newRole = '';
      this.loadRoles();
    });
  }

  onRoleChange(event: any): void {
    const role = event.target.value;
    if (event.target.checked) {
      this.userRoles.push(role);
    } else {
      this.userRoles = this.userRoles.filter(r => r !== role);
    }
  }

  assignRoles(): void {
    if (!this.selectedUserId) {
      return;
    }
    this.roleService.assignRoles(this.selectedUserId, this.userRoles).subscribe();
  }
}
