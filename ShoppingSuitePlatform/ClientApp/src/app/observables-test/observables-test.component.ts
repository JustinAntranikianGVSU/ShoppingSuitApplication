import { Component, OnInit } from '@angular/core';
import { fromEvent, of, interval, concat, merge } from 'rxjs';
import { debounceTime, switchMap, map, takeWhile, delay, tap, exhaustMap, flatMap, mergeMap } from 'rxjs/operators';
import { TodoService } from '../_services/todo.service';
import { TodoEntry } from '../_models/todoEntry';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-observables-test',
  templateUrl: './observables-test.component.html',
  styleUrls: ['./observables-test.component.css']
})
export class ObservablesTestComponent implements OnInit {

	private todoEntry: TodoEntry;
	private debounceTimeInMilliseconds = 2000;
	private fullName = '';
	private fullNameMerged = '';
	public pollingEnabled = true
	public flatMapValue: string = "1"

	constructor(private http: HttpClient, private todoService: TodoService) {
		this.todoEntry = {
      id: 0,
	    title: '',
      userId: 0
    };
	}

	public ngOnInit() {
		this.setUpInputObserver();
		this.setUpNameObserver();
		this.setUpNameObserverMerge();
		this.setUpExhauseMap();
	}

	private setUpInputObserver() {

		var todosObs$ = this.createObserver('switchMapText').pipe(
			map((i: any) => i.currentTarget.value),
			debounceTime(this.debounceTimeInMilliseconds),
			switchMap((value: number) => this.todoService.getTodo(value))
		);

		todosObs$.subscribe(
			(resp: TodoEntry) => this.todoEntry = resp
		);
	}

	private setUpNameObserver() {

		const mergedFunc = (firstNameEvent: any) => {
			return this.createObserver('lastName').pipe(
        map((lastNameEvent: any) => `${firstNameEvent.target.value} ${lastNameEvent.target.value}`)
      )
		};

		var combined$ = this.createObserver('firstName').pipe(mergeMap(mergedFunc));
		combined$.subscribe((fullName: string) => this.fullName = fullName);
	}

	private setUpNameObserverMerge() {

		const extractValue = (input: any): string => input.target.value;
		const observers = ['firstNameMerge', 'lastNameMerge'].map(oo => this.createObserver(oo).pipe(
      map(extractValue)
    ));

		const merged = merge(...observers);
		merged.subscribe((fullName: string) => this.fullNameMerged = fullName);
	}

	private setUpExhauseMap() {

		const exhauseMapClick$ = this.createObserver('exhaustMap', 'click').pipe(
			exhaustMap(this.fakeRequest)
		);

		exhauseMapClick$.subscribe(() => console.log('Exhause Map Value Emitted.'));
	}

	public onConcatMapClicked() {

		var observables$ = [1, 2, 3, 4].map(this.todoService.getTodo); 

		concat(observables$).subscribe(
			(data: any) => console.log('concat operation finished', data),
			null,
			() => console.log('observable finished COMPLETELY')
		)
	}

	public onIntervalClicked() {

		this.pollingEnabled = true;

		var poller$ = interval(2000).pipe(
			takeWhile(val => this.pollingEnabled),
			map((interval) => interval * 10),
    	);

		poller$.subscribe(
			(interval) => console.log('Interval', interval),
			null,
			() => console.log('Polling Complete !')
		)
	}

	public onFlatMapClicked() {

		var todoId = parseInt(this.flatMapValue);

		const observable$ = this.todoService.getTodo(todoId).pipe(
			flatMap((todo: TodoEntry) => todo.id === 1 ? of(todo) : this.todoService.getTodo(todo.id + 1)),
			delay(2000),
			map(({id}: TodoEntry) => id),
		);

		observable$.subscribe(
			(value) => console.log('onFlatMap Next !!', value),
			null,
			() => console.log('onFlatMap Complete !!')
		)
	}

	private fakeRequest() {
		return of(new Date().toUTCString()).pipe(
			tap(_ => console.log('request')),
			delay(3000)
		);
	}

	public onStopPollingClicked = () => this.pollingEnabled = false;

	public onHttpGetClicked = () => this.todoService.getTodo(1).subscribe((resp: TodoEntry) => this.todoEntry = resp);

	private createObserver = (elementId: string, event: string = 'keyup') => fromEvent(document.getElementById(elementId), event);

}
