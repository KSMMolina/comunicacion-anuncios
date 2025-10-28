import { CommonModule } from '@angular/common';
import { Component, computed, signal } from '@angular/core';

@Component({
  selector: 'app-contacts',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './contacts.html',
  styleUrl: './contacts.scss',
})
export class Contacts {
  protected readonly isOpen = signal(false);

  protected readonly contacts = signal([
    {
      id: '1',
      name: 'Laura Hernández',
      avatar: 'https://i.pravatar.cc/150?img=58',
      status: 'Disponible',
      lastMessage: '¿Confirmamos asistencia al evento?',
      unread: 2,
      role: 'Administración'
    },
    {
      id: '2',
      name: 'Comité de Cultura',
      avatar: 'https://i.pravatar.cc/150?img=63',
      status: 'Planificando festival',
      lastMessage: 'Ya tenemos confirmadas las bandas.',
      unread: 5,
      role: 'Residente'
    },
    {
      id: '3',
      name: 'Carlos Seguridad',
      avatar: 'https://i.pravatar.cc/150?img=21',
      status: 'En ronda',
      lastMessage: 'Reporte de incidente cerrado.',
      unread: 0,
      role: 'Seguridad'
    },
    {
      id: '4',
      name: 'Mesa Directiva',
      avatar: 'https://i.pravatar.cc/150?img=15',
      status: 'Reunión en curso',
      lastMessage: 'Pendientes del informe mensual.',
      unread: 1,
      role: 'Administración'
    }
  ]);

  protected readonly unreadTotal = computed(() =>
    this.contacts().reduce((total, contact) => total + contact.unread, 0)
  );

  protected toggle(): void {
    this.isOpen.update((value) => !value);
  }

}
